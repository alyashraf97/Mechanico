using Microsoft.Extensions.Configuration;
using System.Collections.Concurrent;

public class Program
{
    public static void Main(string[] args)
    {
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
    Host.CreateDefaultBuilder(args)
        .UseSystemd() // add this
        .ConfigureServices((hostContext, services) =>
        {
            var config = new Configuration("Cli.yaml");
            services.AddHostedService<SSHWorker>();
        });
}