using CryptoTrading.Data.Context;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Options;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CryptoTrading.Data
{
    public class TemporaryDbContextFactory : IDbContextFactory<CryptoTradingContext>
    {
        public CryptoTradingContext CreateDbContext()
        {
            var builder = new DbContextOptionsBuilder<CryptoTradingContext>();
            builder.UseSqlServer("Data Source=.;Initial Catalog=CryptoTradingIdentity;Integrated Security=True;",
                optionsBuilder => optionsBuilder.MigrationsAssembly(typeof(CryptoTradingContext).GetTypeInfo().Assembly.GetName().Name));
            return new CryptoTradingContext(builder.Options);
        }
    }

    public class TemporaryDbContextFactoryScopes : IDbContextFactory<PersistedGrantDbContext>
    {
        public PersistedGrantDbContext CreateDbContext()
        {
            var builder = new DbContextOptionsBuilder<PersistedGrantDbContext>();
            builder.UseSqlServer("Data Source=.;Initial Catalog=CryptoTradingIdentity;Integrated Security=True;",
               optionsBuilder => optionsBuilder.MigrationsAssembly(typeof(CryptoTradingContext).GetTypeInfo().Assembly.GetName().Name));
            return new PersistedGrantDbContext(builder.Options, new OperationalStoreOptions());
        }
    }
    public class TemporaryDbContextFactoryOperational : IDbContextFactory<ConfigurationDbContext>
    {
        public ConfigurationDbContext CreateDbContext()
        {
            var builder = new DbContextOptionsBuilder<ConfigurationDbContext>();
            builder.UseSqlServer("Data Source=.;Initial Catalog=CryptoTradingIdentity;Integrated Security=True;",
                 optionsBuilder => optionsBuilder.MigrationsAssembly(typeof(CryptoTradingContext).GetTypeInfo().Assembly.GetName().Name));

            return new ConfigurationDbContext(builder.Options, new ConfigurationStoreOptions());
        }
    }
}
