using CouturierVision.Domain.Entities;
using CouturierVision.Domain.Enums;
using CouturierVision.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CouturierVision.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public DbSet<Client> Clients { get; set; } = null!;
    public DbSet<Order> Orders { get; set; } = null!;
    public DbSet<Appointment> Appointments { get; set; } = null!;

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Client>(entity =>
        {
            entity.HasKey(c => c.Id);
            entity.Property(c => c.FirstName).IsRequired().HasMaxLength(100);
            entity.Property(c => c.LastName).IsRequired().HasMaxLength(100);
            entity.Property(c => c.PhoneNumber).HasMaxLength(20);
            entity.Property(c => c.StylePreferences).HasMaxLength(500);

            entity.Property(c => c.Email)
                .HasConversion(e => e.Value, v => new Email(v))
                .HasMaxLength(256)
                .IsRequired();

            entity.HasIndex(c => c.Email).IsUnique();

            entity.Property(c => c.Measurements)
                .HasConversion(
                    m => m == null ? null : m.Json,
                    v => v == null ? null : new Measurements(v))
                .HasColumnName("MeasurementsJson");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(o => o.Id);
            entity.Property(o => o.TotalPrice).HasColumnType("decimal(18,2)");
            entity.Property(o => o.DepositPaid).HasColumnType("decimal(18,2)");
            entity.Property(o => o.MeasurementsJson)
                .HasColumnType("jsonb");
            entity.Property(o => o.Status)
                .HasConversion(new EnumToStringConverter<OrderStatus>());
        });

        modelBuilder.Entity<Appointment>(entity =>
        {
            entity.HasKey(a => a.Id);
            entity.Property(a => a.Type).IsRequired().HasMaxLength(100);
        });
    }
}
