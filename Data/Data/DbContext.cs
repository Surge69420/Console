using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Models;
using Data.Services;
using Microsoft.EntityFrameworkCore;

namespace Data.Data
{
    class serverDbContext : DbContext
    {
        public DbSet<TaxPayer> TaxPayers { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer($"data source=DESKTOP-7R8OIVU\\SQLEXPRESS01;initial catalog=TaxPayers;Trusted_Connection=True;TrustServerCertificate=True");
        }
    }
}

