using System;
using System.Net;
using CommandLine;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using NAudio.Wave;
using VoicevoxEngineSharp.Core.Acoustic.Usecases;
using VoicevoxEngineSharp.Core.Language.Providers;
using VoicevoxEngineSharp.Core.Language.Usecases;
using VoicevoxEngineSharp.Core.Usecases;

var parseResult = Parser.Default.ParseArguments<CommandLineOptions>(args);
Func<ParserResult<CommandLineOptions>, CommandLineOptions> ifParseFailed = (result) =>
{
    var notParsed = result as NotParsed<CommandLineOptions>;
    if (notParsed.Errors.IsHelp() || notParsed.Errors.IsVersion())
    {
        return null;
    }
    Environment.Exit(1);
    return null;
};

var parsedOptions = parseResult.Tag switch
{
    ParserResultType.Parsed => (parseResult as Parsed<CommandLineOptions>).Value,
    ParserResultType.NotParsed => ifParseFailed(parseResult),
    _ => throw new InvalidOperationException()
};
if (parsedOptions == null)
{
    return;
}

const string OpenJTalkDictPath = @"open_jtalk_dic_utf_8-1.11";
var dictPath = parsedOptions.VoicevoxDir == null
    ? Path.Join(Directory.GetCurrentDirectory(), OpenJTalkDictPath)
    : Path.Join(parsedOptions.VoicevoxDir, "pyopenjtalk", OpenJTalkDictPath);

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "voicevox_engine_sharp", Version = "v3" });
});

builder.Services.AddSingleton<IFullContextProvider>(new FullContextProvider(dictPath));
builder.Services.AddSingleton<TextToUtterance>();
if (parsedOptions.VoicevoxDir != null)
{
    Directory.SetCurrentDirectory(parsedOptions.VoicevoxDir);
}

builder.Services.AddSingleton<SynthesisEngine>(SynthesisEngineBuilder.Initialize("1", "2", "3", parsedOptions.UseGpu, parsedOptions.UseCore));
builder.Services.AddSingleton<Synthesis>();
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "MyPolicy",
        builder =>
        {
            builder.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
        });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseCors("MyPolicy");
app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "voicevox_engine_sharp v1"));

app.MapPost("/audio_query", (int speaker, string text, Synthesis synthesisService) =>
{
    var accentPhrases = synthesisService.CreateAccentPhrases(text, speaker);

    return new AudioQuery
    {
        AccentPhrases = accentPhrases.Select(v => AccentPhrase.FromDomain(v)),
        SpeedScale = 1,
        PitchScale = 0,
        IntonationScale = 1,
        VolumeScale = 1,
        PrePhonemeLength = 0.1f,
        PostPhonemeLength = 0.1f,
        OutputSamlingRate = 24000,
        OutputStereo = false,
    };
});

app.MapPost("/accent_phrases", async (int speaker, string text, Synthesis synthesisService) =>
{
    var accentPhrases = synthesisService.CreateAccentPhrases(text, speaker);

    return accentPhrases.Select(v => AccentPhrase.FromDomain(v));
});

app.MapPost("/mora_pitch", async (int speaker, Synthesis synthesisService, HttpContext context) =>
{
    // This will be supported by default in .NET 6 https://github.com/dotnet/aspnetcore/pull/36118
    if (!context.Request.HasJsonContentType())
    {
        throw new BadHttpRequestException("unsupporterd media type", StatusCodes.Status415UnsupportedMediaType);
    }

    var request = await context.Request.ReadFromJsonAsync<IEnumerable<AccentPhrase>>();
    var accentPhrases = synthesisService.ReplaceMoraPitch(request.Select(v => AccentPhrase.ToDomain(v)), speaker);

    return accentPhrases.Select(v => AccentPhrase.FromDomain(v));
});

app.MapPost("/synthesis", async (int speaker, Synthesis synthesisService, AudioQuery request, HttpContext context) =>
{
   // This will be supported by default in .NET 6 https://github.com/dotnet/aspnetcore/pull/36118
   if (!context.Request.HasJsonContentType())
   {
       throw new BadHttpRequestException("unsupporterd media type", StatusCodes.Status415UnsupportedMediaType);
   }

    var wave = synthesisService.SynthesisWave(new VoicevoxEngineSharp.Core.Acoustic.Models.AudioQuery
    {
        AccentPhrases = request.AccentPhrases.Select(v => AccentPhrase.ToDomain(v)),
        SpeedScale = request.SpeedScale,
        PitchScale = request.PitchScale,
        IntonationScale = request.IntonationScale,
        VolumeScale = request.VolumeScale,
        PrePhonemeLength = request.PrePhonemeLength,
        PostPhonemeLength = request.PostPhonemeLength,
        OutputSamlingRate = request.OutputSamlingRate,
        OutputStereo = request.OutputStereo,
    }, speaker).ToArray();

    var stream = new MemoryStream();
    using var writerStream = new WaveFileWriter(stream, WaveFormat.CreateIeeeFloatWaveFormat(24000, 1));
    writerStream.WriteSamples(wave, 0, wave.Length);
    stream.Position = 0;
    
    return Results.Stream(stream, "audio/wav");
});

app.MapGet("/version", async () =>
{
    // TODO: Use Results.File
    return await System.IO.File.ReadAllTextAsync("VERSION.txt", encoding: System.Text.Encoding.UTF8);
});

app.MapGet("/speakers", async (context) =>
{
    // TODO: Use Results.Text(speakers, "application/json")
    var speakers = System.IO.File.ReadAllTextAsync("speakers.json", encoding: System.Text.Encoding.UTF8);
    await context.Response.WriteAsJsonAsync(speakers);
    return;
});

app.Run($"http://{parsedOptions.Host}:{parsedOptions.Port}");

//

internal class CommandLineOptions
{

    [Option(longName: "host", Required = false, Default = "127.0.0.1")]
    public string Host { get; set; }

    [Option(longName: "port", Required = false, Default = 50021)]
    public int Port { get; set; }

    [Option(longName: "use_gpu", Required = false, Default = false)]
    public bool UseGpu { get; set; }

    [Option(longName: "voicevox_dir", Required = false, Default = null)]
    public string? VoicevoxDir { get; set; }

    // @unused
    // NOTE: each_cpp_forwarderがevalされたDirと同じディレクトリ内のvoicelibを探索するため、
    // DllImport でNativeメソッドを呼び出し出来るようにしている今の作りと組み合わせが良くない
    // 今後作りを変える
    [Option(longName: "voiceliv_dir", Required = false, Default = null)]
    public string? VoicelibDir { get; set; }

    [Option(longName: "use_core", Required = false, Default = false)]
    public bool UseCore { get; set; }
}
