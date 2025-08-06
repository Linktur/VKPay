using Microsoft.EntityFrameworkCore;
using VkPayTest.Models;
using System.Text.Json;
using System.Collections.Generic;

namespace VkPayTest.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<VkPaymentNotification> PaymentNotifications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure VkPaymentNotification entity
            modelBuilder.Entity<VkPaymentNotification>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.CreatedAt).IsRequired();
                entity.Property(e => e.Type).IsRequired().HasMaxLength(50);
                entity.Property(e => e.NotificationType).IsRequired().HasMaxLength(50);
                entity.Property(e => e.AppId).IsRequired().HasMaxLength(50);
                entity.Property(e => e.UserId).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Status).HasMaxLength(50);
                entity.Property(e => e.PaymentStatus).HasMaxLength(50);
                entity.Property(e => e.Currency).HasMaxLength(10).HasDefaultValue("RUB");
                
                // Configure AdditionalData as a JSON column
                entity.Property(e => e.AdditionalData)
                    .HasColumnType("jsonb")
                    .HasConversion(
                        v => JsonSerializer.Serialize(v, new JsonSerializerOptions()),
                        v => JsonSerializer.Deserialize<Dictionary<string, object>>(v, new JsonSerializerOptions()) ?? new Dictionary<string, object>()
                    );
                
                // Indexes for better query performance
                entity.HasIndex(e => new { e.UserId, e.Type });
                entity.HasIndex(e => e.OrderId).IsUnique();
                entity.HasIndex(e => e.SubscriptionId);
                entity.HasIndex(e => e.CreatedAt);
            });
        }
    }
}
