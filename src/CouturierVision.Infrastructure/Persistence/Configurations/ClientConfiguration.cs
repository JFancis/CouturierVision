using CouturierVision.Domain.Entities;
using CouturierVision.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CouturierVision.Infrastructure.Persistence.Configurations;

public class ClientConfiguration : IEntityTypeConfiguration<Client>
{
    public void Configure(EntityTypeBuilder<Client> builder)
    {
        builder.ToTable("clients");

        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id).HasColumnName("id");

        builder.Property(c => c.FirstName)
            .HasColumnName("first_name")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(c => c.LastName)
            .HasColumnName("last_name")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(c => c.Email)
            .HasColumnName("email")
            .HasMaxLength(255)
            .IsRequired()
            .HasConversion(
                e => e.Value,
                v => new Email(v));

        builder.HasIndex(c => c.Email).IsUnique();

        builder.Property(c => c.PhoneNumber)
            .HasColumnName("phone_number")
            .HasMaxLength(20);

        builder.Property(c => c.StylePreferences)
            .HasColumnName("style_preferences")
            .HasMaxLength(500);

        builder.Property(c => c.Measurements)
            .HasColumnName("measurements")
            .HasColumnType("jsonb")
            .HasConversion(
                m => m == null ? null : m.Json,
                v => v == null ? null : new Measurements(v));

        builder.Property(c => c.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();
    }
}
