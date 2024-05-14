﻿
namespace Stockify.Data{
  using Microsoft.EntityFrameworkCore;
using Stockify.Models;


public class StockifyContext :DbContext
{
     public StockifyContext(DbContextOptions<StockifyContext> options)
            : base(options)
        {
        }
          protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
          

            // Relación entre Product y ProductCategory (muchos a muchos)
            modelBuilder.Entity<ProductCategory>()
                .HasKey(pc => new { pc.ProductId, pc.CategoryId });

            modelBuilder.Entity<ProductCategory>()
                .HasOne(pc => pc.Product)
                .WithMany(p => p.ProductCategories)
                .HasForeignKey(pc => pc.ProductId);

            modelBuilder.Entity<ProductCategory>()
                .HasOne(pc => pc.Category)
                .WithMany(c => c.ProductCategories)
                .HasForeignKey(pc => pc.CategoryId);

        }

            public DbSet<Inventory> Inventories { get; set; }
            public DbSet<Category> Categories {get; set;}
            public DbSet<Product> Products {get;set;}
            public DbSet<ProductCategory> ProductsCategory {get; set;}
            public DbSet<Tenant> Tenants {get; set; }
            public DbSet<User> Users {get; set;}    
            public DbSet<Transaction> Transactions {get; set; }
            public DbSet<Role> Roles {get; set;}
        

}
}
