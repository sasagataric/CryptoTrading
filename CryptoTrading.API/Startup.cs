using CryptoTrading.API.Mapper;
using CryptoTrading.Data.Context;
using CryptoTrading.Domain.Interfaces;
using CryptoTrading.Domain.Services;
using CryptoTrading.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using CryptoTrading.Domain.Mapper;

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

            services.AddAutoMapper(typeof(ControllersProfileMapper), typeof(ServicesProfileMapper));

            services.AddHttpClient();

            services.AddTransient<CoinGecko.Interfaces.ICoinGeckoClient, CoinGecko.Clients.CoinGeckoClient>();

            //Repository
            services.AddTransient<ICoinsRepository, CoinsRepository>();
            services.AddTransient<IUsersRepository, UsersRepository>();
            services.AddTransient<IWalletHistoryRepository, WalletHistoryRepository>();
            services.AddTransient<IWalletsRepositor, WalletsRepository>();
            services.AddTransient<IPurchasedCoinsRepository, PurchasedCoinsRepository>();
            services.AddTransient<IWalletHistoryRepository, WalletHistoryRepository>();

            //Service
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IWalletService, WalletService>();
            services.AddTransient<ICoinService, CoinService>();
            services.AddTransient<IPurchasedCoinService, PurchasedCoinService>();
            services.AddTransient<IWalletHistoryService, WalletHistoryService>();



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
