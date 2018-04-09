using System.IO;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace ImageCollections.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("hosting.json", optional: false)
                .Build();

            return WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseUrls(config["server.urls"])
                .ConfigureLogging((c, t) =>
                {
                    Log.Logger = new LoggerConfiguration()
                        .ReadFrom.Configuration(c.Configuration)
                        .CreateLogger();

                    t.AddSerilog(Log.Logger);
                })
                .Build();
        }
    }
}
