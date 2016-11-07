using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using WebbenVNext.Storage;

namespace WebbenVNext
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddEnvironmentVariables();

            if (env.IsDevelopment())
            {
                builder.AddUserSecrets();
            }

            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddOptions();

            // Local blobs
            //services.AddTransient<IBlobs, LocalBlobs>();

            // Azure blobs
            services.AddTransient<IBlobs, AzureBlobs>();
            services.Configure<AzureBlobsOptions>(options =>
            {
                options.StorageConnectionString = Configuration.GetValue<string>("AzureBlobConnectionString");
            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseStaticFiles();
            app.UseMvcWithDefaultRoute();
        }
    }
}
