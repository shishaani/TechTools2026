using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TechTools.Models;

namespace TechTools.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext(options)
    {
        public DbSet<GPU> GPUs { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Seed initial data for GPUs
            builder.Entity<GPU>().HasData(
                new GPU { Id = 1, Brand = "NVIDIA", Model = "GeForce RTX 3080", Price = 1590.00m, Picture = "", Description = "High-end gaming GPU with excellent performance." },
                new GPU { Id = 2, Brand = "AMD", Model = "Radeon RX 6800 XT", Price = 699.00m, Picture = "", Description = "Powerful GPU with great value for gaming." }
            );
        }
    }
}
