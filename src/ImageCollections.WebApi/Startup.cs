using ImageCollections.WebApi.Configuration;
using ImageCollections.WebApi.Infrastructure;
using ImageCollections.WebApi.Managers;
using ImageCollections.WebApi.Managers.HashGenerator;
using ImageCollections.WebApi.Managers.StorageFactory;
using ImageCollections.WebApi.Repositories.FileSystemStorage;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ImageCollections.WebApi
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<FileRepositorySetting>(_configuration.GetSection("FileRepository"));
            services.AddScoped<IStorageFactory, StorageFactory>();
            services.AddScoped<IHashGenerator, HashGenerator>();
            services.AddScoped<IFileSystemStorage, FileSystemStorage>();
            services.AddScoped<IPathGenerator, PathGenerator>();
            services.AddScoped<ICollectionManager, CollectionManager>();
            services.AddScoped<IImageManager, ImageManager>();
            services.AddRabbitMQ(_configuration);
            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseMvc();
        }
    }
}
