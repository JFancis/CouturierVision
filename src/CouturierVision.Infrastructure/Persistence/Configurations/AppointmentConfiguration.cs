using CouturierVision.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CouturierVision.Infrastructure.Persistence.Configurations;

public class AppointmentConfiguration : IEntityTypeConfiguration<Appointment>
{
    public void Configure(EntityTypeBuilder<Appointment> builder)
    {
        builder.ToTable("appointments");

        builder.HasKey(a => a.Id);
        builder.Property(a => a.Id).HasColumnName("id");

        builder.Property(a => a.ClientId).HasColumnName("client_id").IsRequired();
        builder.Property(a => a.ArtisanId).HasColumnName("artisan_id").IsRequired();

        builder.Property(a => a.Type)
            .HasColumnName("type")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(a => a.StartTime)
            .HasColumnName("start_time")
            .IsRequired();

        builder.Property(a => a.EndTime)
            .HasColumnName("end_time")
            .IsRequired();
    }
}
