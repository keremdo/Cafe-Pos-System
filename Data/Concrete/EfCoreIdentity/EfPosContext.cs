using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dmlcafepos.Entities;
using DmlCafePos.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Dmlcafepos.Data.Concrete.EfCoreIdentity
{
    public class EfPosContext : IdentityDbContext<AppUser,AppRole,string>
    {
        public DbSet<Area> Areas {get;set;}
        public DbSet<Order> Orders {get;set;}
        public DbSet<Product> Products {get;set;}
        public DbSet<Table> Tables {get;set;}
        public DbSet<Category> Categories {get;set;}
        public DbSet<Customer> Customers {get;set;}
        public DbSet<Payment> Payments {get;set;}
        public EfPosContext(DbContextOptions<EfPosContext> options):base(options)
        {
            
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Area>().HasKey(e => e.AreaId);
            modelBuilder.Entity<Order>().HasKey(e => e.OrderId);
            modelBuilder.Entity<Product>().HasKey(e => e.ProductId);
            modelBuilder.Entity<Table>().HasKey(e => e.TableId);
            modelBuilder.Entity<Category>().HasKey(e => e.CategoryId);
            modelBuilder.Entity<Payment>().HasKey(e => e.PaymentId);
            base.OnModelCreating(modelBuilder);
        }
    }
}