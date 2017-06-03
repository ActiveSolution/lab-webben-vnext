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

        public IConfigurationRoot Configuration { get; }

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddEnvironmentVariables();

            if (env.IsDevelopment())
            {
                builder.AddUserSecrets<Startup>();
            }
            else
            {
                builder.AddJsonFile($"appsettings.json", optional: true);
            }

            Configuration = builder.Build();
        }


        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddOptions();

            // Local blobs
            services.AddTransient<IBlobs, AzureBlobs>();

            var connString = Configuration.GetValue<string>("AzureBlobConnectionString");
            services.Configure<AzureBlobsOptions>(options =>
            {
                options.StorageConnectionString = connString;
            });


        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();
            app.UseMvcWithDefaultRoute();

        }
    }
}
