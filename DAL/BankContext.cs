﻿using DomainModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL
{
    public class BankContext : DbContext
    {
        public DbSet<Client> Clients { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Manager> Managers { get; set; }
        public DbSet<Deposit> Deposits { get; set; }
        public DbSet<Saving> Savings { get; set; }
        public DbSet<Card> Cards { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=.\SQLEXPRESS;Initial Catalog= BankDb; Integrated Security=True");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Client>().ToTable("Clients", "BCR");
            modelBuilder.Entity<Employee>().ToTable("Employees", "BCR");
            modelBuilder.Entity<Manager>().ToTable("Managers", "BCR");
            modelBuilder.Entity<Deposit>().ToTable("Deposits", "CB");
            modelBuilder.Entity<Saving>().ToTable("Savings", "CB");
            modelBuilder.Entity<Card>().ToTable("Cards", "CB");

            modelBuilder.Entity<Client>()
                .HasOne(c => c.MyEmployee)
                .WithMany(e => e.MyClients);

            modelBuilder.Entity<Client>()
                .HasMany(c => c.MyAccounts)
                .WithOne(a => a.AccountOwner)
                .IsRequired(false);

            modelBuilder.Entity<Manager>()
                .HasMany(m => m.MyEmployees)
                .WithOne(e => e.MyManager);

            modelBuilder.Entity<Card>()
                .HasOne(c => c.CardDeposit);

            modelBuilder.Entity<Deposit>()
                .HasMany(d => d.DepositCards)
                .IsRequired(false);

            base.OnModelCreating(modelBuilder);
        }
    }
}
