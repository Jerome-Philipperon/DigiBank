using DomainModel;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL
{
    public class BankContext : IdentityDbContext
    {
        public DbSet<Client> Clients { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Manager> Managers { get; set; }
        public DbSet<Person> People { get; set; }
        public DbSet<Deposit> Deposits { get; set; }
        public DbSet<Saving> Savings { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Card> Cards { get; set; }

        public BankContext()
            : base()
        {

        }

        public BankContext(DbContextOptions options)
            : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseSqlServer(@"Data Source=.\SQLEXPRESS;Initial Catalog= BankDb; Integrated Security=True"); //For Consol app
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>().ToTable("Accounts", "CB");
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
                .HasOne(c => c.CardDeposit)
                .WithMany(d => d.DepositCards)
                .IsRequired(false);

            base.OnModelCreating(modelBuilder);
        }
    }
}
