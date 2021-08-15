using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Microsoft.Extensions.DependencyInjection;
using CommandLine;

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
