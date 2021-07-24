using eShopSolution.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.Data.EF
{
    public class EShopDBContext : DbContext
    {
        // constructor, enter a DB option      
        public EShopDBContext(DbContextOptions options) : base(options)
        {
        }

        // fundamental allow us use a connection string and declare entities
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
    }
}
