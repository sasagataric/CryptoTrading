using CryptoTrading.Data.Context;
using CryptoTrading.Data.Entities;
using CryptoTrading.IdentityServer.ExternalGrant;
using CryptoTrading.IdentityServer.Interfaces;
using CryptoTrading.IdentityServer.Interfaces.Processors;
using CryptoTrading.IdentityServer.Processors;
using CryptoTrading.IdentityServer.Providers;
using CryptoTrading.IdentityServer.Repository;
using CryptoTrading.IdentityServer.Repository.Interfaces;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;

namespace CryptoTrading.IdentityServer.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDatabaseConfiguration(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<CryptoTradingContext>(o => o
                .UseSqlServer(connectionString,
                        optionsBuilder => optionsBuilder.MigrationsAssembly(typeof(CryptoTradingContext).GetTypeInfo().Assembly.GetName().Name)));
            
            services.AddIdentity<User, AppRole>()
               .AddEntityFrameworkStores<CryptoTradingContext>()
               .AddDefaultTokenProviders();

            return services;
        }

        public static IServiceCollection AddIdentityServerConfig(this IServiceCollection services, string connectionString)
        {
            services.AddIdentityServer()
                .AddDeveloperSigningCredential()
                .AddConfigurationStore(options =>
                {
                    options.ConfigureDbContext = b => b.UseSqlServer(connectionString,
                        sql => sql.MigrationsAssembly(typeof(CryptoTradingContext).GetTypeInfo().Assembly.GetName().Name));
                })
                .AddOperationalStore(options =>
                {
                    options.ConfigureDbContext = b => b.UseSqlServer(connectionString,
                        sql => sql.MigrationsAssembly(typeof(CryptoTradingContext).GetTypeInfo().Assembly.GetName().Name));
                })
                .AddAspNetIdentity<User>();

            return services;
        }
        public static IServiceCollection AddServices<TUser>(this IServiceCollection services) where TUser : User, new()
        {
            services.AddScoped<INonEmailUserProcessor, NonEmailUserProcessor<TUser>>();
            services.AddScoped<IEmailUserProcessor, EmailUserProcessor<TUser>>();
            services.AddScoped<IExtensionGrantValidator, ExternalAuthenticationGrant<TUser>>();
            services.AddSingleton<HttpClient>();
            return services;
        }

        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IProviderRepository, ProviderRepository>();
            return services;
        }

        public static IServiceCollection AddProviders<TUser>(this IServiceCollection services) where TUser : User, new()
        {
           services.AddTransient<IGoogleAuthProvider, GoogleAuthProvider<TUser>>();
           return services;
        }
    }
}
