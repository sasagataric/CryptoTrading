using CryptoTrading.API.Mapper;
using CryptoTrading.Data.Context;
using CryptoTrading.Domain.Common;
using CryptoTrading.Domain.Interfaces;
using CryptoTrading.Domain.Services;
using CryptoTrading.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CryptoTrading.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<CryptoTradingContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("CryptoTradingConnection"))

            );
           
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "CryptoTrading.API", Version = "v1" });
            });

            services.AddAutoMapper(typeof(ContollersProfileMapper), typeof(ServicesProfileMapper));

            services.AddHttpClient();

            services.AddTransient<CoinGecko.Interfaces.ICoinGeckoClient, CoinGecko.Clients.CoinGeckoClient>();

            //Repository
            services.AddTransient<ICoinsRepository, CoinsRepository>();
            services.AddTransient<IUsersRepository, UsersRepository>();
            services.AddTransient<IWalletHistoryRepository, WalletHistoryRepository>();
            services.AddTransient<IWalletssRepositor, WalletsRepository>();

            //Service
            services.AddTransient<IUserService, UserService>();


            services.AddCors(options => {
                options.AddPolicy("CorsPolicy",
                    corsBuilder => corsBuilder.WithOrigins("http://localhost:3000")
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials());
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "CryptoTrading.API v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
