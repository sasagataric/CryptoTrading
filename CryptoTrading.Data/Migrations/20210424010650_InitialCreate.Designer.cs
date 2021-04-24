﻿// <auto-generated />
using System;
using CryptoTrading.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CryptoTrading.Data.Migrations
{
    [DbContext(typeof(CryptoTradingContext))]
    [Migration("20210424010650_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.5")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("CoinUser", b =>
                {
                    b.Property<string>("CoinsId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<Guid>("UsersId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("CoinsId", "UsersId");

                    b.HasIndex("UsersId");

                    b.ToTable("CoinUser");
                });

            modelBuilder.Entity("CryptoTrading.Data.Entities.Coin", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Image")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Symbol")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Coins");
                });

            modelBuilder.Entity("CryptoTrading.Data.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Role")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("CryptoTrading.Data.Entities.Wallet", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<double>("Balance")
                        .HasColumnType("float");

                    b.Property<double?>("Profit")
                        .HasColumnType("float");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("UserId1")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("UserId1");

                    b.ToTable("Wallets");
                });

            modelBuilder.Entity("CryptoTrading.Data.Entities.WalletCoins", b =>
                {
                    b.Property<string>("CoinId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<Guid>("WalletId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<double>("Amount")
                        .HasColumnType("float");

                    b.HasKey("CoinId", "WalletId");

                    b.HasIndex("WalletId");

                    b.ToTable("WalletCoins");
                });

            modelBuilder.Entity("CryptoTrading.Data.Entities.WalletHistory", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CoinId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<double>("CoinPrice")
                        .HasColumnType("float");

                    b.Property<DateTime>("TransactionDate")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("WalletId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<double>("amount")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.HasIndex("CoinId");

                    b.HasIndex("WalletId");

                    b.ToTable("WalletHistorys");
                });

            modelBuilder.Entity("CoinUser", b =>
                {
                    b.HasOne("CryptoTrading.Data.Entities.Coin", null)
                        .WithMany()
                        .HasForeignKey("CoinsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CryptoTrading.Data.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("UsersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("CryptoTrading.Data.Entities.Wallet", b =>
                {
                    b.HasOne("CryptoTrading.Data.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId1");

                    b.Navigation("User");
                });

            modelBuilder.Entity("CryptoTrading.Data.Entities.WalletCoins", b =>
                {
                    b.HasOne("CryptoTrading.Data.Entities.Coin", "Coin")
                        .WithMany("WalletCoins")
                        .HasForeignKey("CoinId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CryptoTrading.Data.Entities.Wallet", "Wallet")
                        .WithMany("WalletCoins")
                        .HasForeignKey("WalletId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Coin");

                    b.Navigation("Wallet");
                });

            modelBuilder.Entity("CryptoTrading.Data.Entities.WalletHistory", b =>
                {
                    b.HasOne("CryptoTrading.Data.Entities.Coin", "Coin")
                        .WithMany()
                        .HasForeignKey("CoinId");

                    b.HasOne("CryptoTrading.Data.Entities.Wallet", "Wallet")
                        .WithMany()
                        .HasForeignKey("WalletId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Coin");

                    b.Navigation("Wallet");
                });

            modelBuilder.Entity("CryptoTrading.Data.Entities.Coin", b =>
                {
                    b.Navigation("WalletCoins");
                });

            modelBuilder.Entity("CryptoTrading.Data.Entities.Wallet", b =>
                {
                    b.Navigation("WalletCoins");
                });
#pragma warning restore 612, 618
        }
    }
}
