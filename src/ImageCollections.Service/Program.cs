using System.IO;
using System.Threading;
using ImageCollections.Service.Configuration;
using ImageCollections.Service.Infrastructure;
using ImageCollections.Service.Managers;
using ImageCollections.Service.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace ImageCollections.Service
{
    class Program
    {
        static void Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();

            var services = new ServiceCollection();

            services.AddScoped<IImageCollectionManager, ImageCollectionManager>();
            services.AddScoped<IImageCollectionRepository, ImageCollectionRepository>();
            services.AddScoped<IBusSubscriber, BusSubscriber>();

            services.AddOptions();
            services.Configure<ConnectionStrings>(s =>
            {
                s.SqliteFile = configuration.GetConnectionString("SqliteFile");
            });

            services.AddRabbitMQ(configuration);

            var serviceProvider = services.BuildServiceProvider();

            var busSubscriber = serviceProvider.GetRequiredService<IBusSubscriber>();
            using (busSubscriber.Subscribe())
            {
                Log.Information("Service started");

                var cancelSource = new CancellationTokenSource();
                cancelSource.Token.WaitHandle.WaitOne();
            }
        }
    }
}