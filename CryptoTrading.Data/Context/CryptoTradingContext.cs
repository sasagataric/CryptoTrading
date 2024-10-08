﻿using CryptoTrading.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace CryptoTrading.Data.Context
{
    public class CryptoTradingContext : IdentityDbContext<User,AppRole,Guid>
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Wallet> Wallets { get; set; }
        public DbSet<Coin> Coins { get; set; }
        public DbSet<Holding> Holdings { get; set; }
        public DbSet<WalletHistory> WalletHistorys { get; set; }
        
        public CryptoTradingContext(DbContextOptions<CryptoTradingContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Wallet>()
                .HasMany(c => c.Coins)
                .WithMany(w => w.Wallets)
                .UsingEntity<Holding>(
                    x => x.HasOne(wc => wc.Coin)
                          .WithMany(c => c.PurchasedCoin)
                          .HasForeignKey(wc => wc.CoinId),
                    x => x.HasOne(wc => wc.Wallet)
                          .WithMany(w => w.PurchasedCoin)
                          .HasForeignKey(wc => wc.WalletId),
                    x => x.HasKey(wc => new { wc.WalletId, wc.CoinId })
                    );

            modelBuilder.Entity<User>()
                .HasMany(u => u.Coins)
                .WithMany(c => c.Users)
                .UsingEntity(join => join.ToTable("WatchList"));
        }
    }
}
