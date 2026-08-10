using Lagerverwaltung.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace Lagerverwaltung.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Article> articles { get; set; }

        public DbSet<StockMovement> stockMovements { get; set; }

        public DbSet<Invoice> invoices { get; set; }

        public DbSet<InvoiceItem> invoiceItems { get; set; }

        public DbSet<CompanySettings> companySettings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<StockMovement>()
                .HasOne(s => s.Article)
                .WithMany(a => a.StockMovements)
                .HasForeignKey(s => s.ArticleId);

            modelBuilder.Entity<InvoiceItem>()
              .HasOne(i => i.Invoice)
              .WithMany(i => i.Items)
              .HasForeignKey(i => i.InvoiceId)
              .OnDelete(DeleteBehavior.Cascade);
        }
    }
}