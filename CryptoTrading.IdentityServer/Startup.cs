using CryptoTrading.Data.Context;
using CryptoTrading.Data.Entities;
using CryptoTrading.IdentityServer.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace CryptoTrading.IdentityServer
{
    public class Startup
    {
        private readonly IConfiguration Configuration;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

         public void ConfigureServices(IServiceCollection services)
        {
            services.AddDatabaseConfiguration(Configuration["ConnectionStrings:CryptoTradingConnection"])
                    .AddIdentityServerConfig(Configuration["ConnectionStrings:CryptoTradingConnection"])
                    .AddServices<User>()
                    .AddRepositories()
                    .AddProviders<User>();
        }

          public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseIdentityServer();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", context =>
                {
                    context.Response.Redirect(".well-known/openid-configuration", permanent: false);
                    return Task.CompletedTask;
                });
            });
        }
    }
}
