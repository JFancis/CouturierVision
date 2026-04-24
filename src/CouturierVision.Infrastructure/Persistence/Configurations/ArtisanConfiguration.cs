using CouturierVision.Domain.Entities;
using CouturierVision.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CouturierVision.Infrastructure.Persistence.Configurations;

public class ArtisanConfiguration : IEntityTypeConfiguration<Artisan>
{
    public void Configure(EntityTypeBuilder<Artisan> builder)
    {
        builder.ToTable("artisans");

        builder.HasKey(a => a.Id);
        builder.Property(a => a.Id).HasColumnName("id");

        builder.Property(a => a.FirstName)
            .HasColumnName("first_name")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(a => a.LastName)
            .HasColumnName("last_name")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(a => a.Email)
            .HasColumnName("email")
            .HasMaxLength(255)
            .IsRequired()
            .HasConversion(
                e => e.Value,
                v => new Email(v));

        builder.HasIndex(a => a.Email).IsUnique();

        builder.Property(a => a.PhoneNumber)
            .HasColumnName("phone_number")
            .HasMaxLength(20);

        builder.Property(a => a.Specialization)
            .HasColumnName("specialization")
            .HasMaxLength(200);

        builder.Property(a => a.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();
    }
}
