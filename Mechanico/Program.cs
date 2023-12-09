using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.Diagnostics;

public class Program
{
    public static void Main(string[] args)
    {
        try
        {
            Log.Logger = new LoggerConfiguration().WriteTo.File("Cli.log").CreateLogger();

            Log.Information("Starting application...");

            CreateHostBuilder(args).Build().Run();
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Application terminated unexpectedly.");
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .UseSystemd().UseSerilog()
            .ConfigureServices((hostContext, services) =>
            {
                var config = Configuration.Load("appsettings.json", "hosts");
                services.AddSingleton(config);
                services.AddSingleton(new Dictionary<Machine, Job>());
                services.AddHostedService<SSHWorker>();
            });
}

//< Copy SourceFiles = "$(ProjectDir)Cli.yaml" DestinationFolder = "$(OutDir)" />