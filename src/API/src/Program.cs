using System;
using CommandLine;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
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

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "voicevox_engine_sharp", Version = "v3" });
});
builder.Services.AddSingleton<IFullContextProvider>(new FullContextProvider(@"DICT_PATH", @"HTS_MODEL_PATH"));
builder.Services.AddSingleton<TextToUtterance>();
builder.Services.AddSingleton<SynthesisEngine>(SynthesisEngineBuilder.Initialize("1", "2", "3", false));
builder.Services.AddSingleton<Synthesis>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "voicevox_engine_sharp v1"));

app.MapPost("/audio_query", () =>
{
    return new StatusCodeResult(500);
});

app.MapPost("/accent_phrases", () =>
{
    return new StatusCodeResult(500);
});

app.MapPost("/mora_pitch", () =>
{
    return new StatusCodeResult(500);
});

app.MapPost("/synthesis", () =>
{
    return new StatusCodeResult(500);
});

app.MapGet("/version", () => "Hello World!");

app.MapGet("/debug/extract_labels", async (context) =>
{
    if (!context.Request.Query.TryGetValue("text", out var parseText))
    {
        context.Response.StatusCode = 200;
        return;
    }

    var contextProvider = context.RequestServices.GetService<IFullContextProvider>();
    var labels = contextProvider.ToFullContextLabels(parseText.ToString());

    await context.Response.WriteAsJsonAsync(new Dictionary<string, object>() { { "labels", labels } });
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
}
