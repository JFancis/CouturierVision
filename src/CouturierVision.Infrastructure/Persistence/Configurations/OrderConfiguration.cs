using CouturierVision.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CouturierVision.Infrastructure.Persistence.Configurations;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("orders");

        builder.HasKey(o => o.Id);
        builder.Property(o => o.Id).HasColumnName("id");

        builder.Property(o => o.ClientId).HasColumnName("client_id").IsRequired();

        builder.Property(o => o.Status)
            .HasColumnName("status")
            .HasConversion<string>()
            .IsRequired();

        builder.Property(o => o.TotalPrice)
            .HasColumnName("total_price")
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(o => o.DepositPaid)
            .HasColumnName("deposit_paid")
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(o => o.MeasurementsJson)
            .HasColumnName("measurements_json")
            .HasColumnType("jsonb");

        builder.Property(o => o.Deadline)
            .HasColumnName("deadline")
            .IsRequired();

        builder.Property(o => o.AssignedArtisanId)
            .HasColumnName("assigned_artisan_id");
    }
}
