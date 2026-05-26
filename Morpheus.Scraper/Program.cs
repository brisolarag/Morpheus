using Morpheus.Scraper;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console()
    .WriteTo.File("logs/scraper-.txt", rollingInterval: RollingInterval.Day) // Creates a new file every day
    .CreateLogger();

try
{
    Log.Information("Starting up the Morpheus Scraper Host...");

    var builder = Host.CreateApplicationBuilder(args);

    builder.Services.AddSerilog();

    builder.Services.AddHttpClient();
    builder.Services.AddHostedService<Worker>();

    var host = builder.Build();
    host.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "The scraper host terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}