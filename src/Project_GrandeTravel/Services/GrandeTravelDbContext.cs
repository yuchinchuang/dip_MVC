using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using Project_GrandeTravel.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Project_GrandeTravel.Services
{
    public class GrandeTravelDbContext:IdentityDbContext<MyUser>
    {
        public DbSet<Category> TblCategory { get; set; }
        public DbSet<Package> TblPackage { get; set; }
        public DbSet<CustomerProfile> TblCustomerProfile { get; set; }
        public DbSet<ProviderProfile> TblProviderProfile { get; set; }
        public DbSet<Order> TblOrder { get; set; }
        public DbSet<Feedback> TblFeedback { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder option)
        {
            option.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=GrandeTravelDB2;Trusted_Connection=True;");
        }
    }
}
