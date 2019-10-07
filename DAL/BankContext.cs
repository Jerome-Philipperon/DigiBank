using DomainModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL
{
    public class BankContext : DbContext
    {
        public DbSet<Client> Clients { get; set; }
    }
}
