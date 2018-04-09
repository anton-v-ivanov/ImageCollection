using EasyNetQ;
using EasyNetQ.Serilog;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace ImageCollections.Service.Infrastructure
{
    public static class RabbitMQInitializer
    {
        public static IServiceCollection AddRabbitMQ(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration["RabbitMQ:ConnectionString"];
            IEasyNetQLogger easyNetQLogger = new SerilogLogger(Log.Logger);

            var bus = RabbitHutch.CreateBus(connectionString, s => { s.Register(_ => easyNetQLogger); });
            return services.AddSingleton(bus);
        }
    }
}