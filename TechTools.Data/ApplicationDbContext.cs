using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TechTools.Models;

namespace TechTools.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext(options)
    {
        public DbSet<GPU> GPUs { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<CPU> CPUs { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Review>()
                .HasOne(review => review.GPU)
                .WithMany()
                .HasForeignKey(review => review.GPUId)
                .OnDelete(DeleteBehavior.Cascade);

            // Seed initial data for GPUs
            builder.Entity<GPU>().HasData(
                new GPU { Id = 1, Brand = "NVIDIA", Model = "GeForce RTX 3080", Price = 1590.00m, Picture = "", Description = "High-end gaming GPU with excellent performance." },
                new GPU { Id = 2, Brand = "AMD", Model = "Radeon RX 6800 XT", Price = 699.00m, Picture = "", Description = "Powerful GPU with great value for gaming." }
            );

            // Seed initial data for CPUs
            builder.Entity<CPU>().HasData(
                new CPU { Id = 1, Brand = "Intel", Model = "Intel Core i9 14900K", Price = 489.00m, Picture = "", Description = "A 24‑core, 6.0 GHz high‑end CPU made for top‑tier gaming and heavy workloads." },
                new CPU { Id = 2, Brand = "AMD", Model = "Ryzen 7 7800X3D", Price = 354.90m, Picture = "", Description = "The fastest gaming CPU with 3D V‑Cache and top FPS efficiency." }
            );
        }
    }
}
