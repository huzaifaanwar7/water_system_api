using Microsoft.EntityFrameworkCore;
using GBS.Api.DbModels;

namespace GBS.Api.Data
{
    public class GBS_DbContext : DbContext
    {
        public GBS_DbContext(DbContextOptions<GBS_DbContext> options)
            : base(options)
        {
        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Delivery> Deliveries { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<User> Users { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure relationships if needed
            modelBuilder.Entity<Delivery>()
                .HasOne(d => d.Customer)
                .WithMany(c => c.Deliveries)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Payment>()
                .HasOne(p => p.Customer)
                .WithMany(c => c.Payments)
                .HasForeignKey(p => p.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Invoice>()
                .HasOne(i => i.Customer)
                .WithMany(c => c.Invoices)
                .HasForeignKey(i => i.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Delivery>()
                .HasOne(d => d.Invoice)
                .WithMany(i => i.Deliveries)
                .HasForeignKey(d => d.InvoiceId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
