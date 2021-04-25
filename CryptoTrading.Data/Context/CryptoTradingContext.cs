using CryptoTrading.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoTrading.Data.Context
{
    public class CryptoTradingContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Wallet> Wallets { get; set; }
        public DbSet<Coin> Coins { get; set; }
        public DbSet<WalletCoins> WalletCoins { get; set; }
        public DbSet<WalletHistory> WalletHistorys { get; set; }
        
        public CryptoTradingContext(DbContextOptions<CryptoTradingContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Wallet>()
                .HasMany(c => c.Coins)
                .WithMany(w => w.Wallets)
                .UsingEntity<WalletCoins>(
                x => x.HasOne(w => w.Coin).WithMany(c=>c.WalletCoins).HasForeignKey("CoinId"),
                x => x.HasOne(w => w.Wallet).WithMany(w => w.WalletCoins).HasForeignKey("WalletId"));

            modelBuilder.Entity<User>()
                .HasMany(u => u.Coins)
                .WithMany(c => c.Users)
                .UsingEntity(join => join.ToTable("WatchList"));
        }
    }
}
