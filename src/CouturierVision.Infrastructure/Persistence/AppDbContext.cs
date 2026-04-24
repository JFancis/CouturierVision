using CouturierVision.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CouturierVision.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public DbSet<Client> Clients { get; set; } = null!;
    public DbSet<Order> Orders { get; set; } = null!;
    public DbSet<Appointment> Appointments { get; set; } = null!;
    public DbSet<Artisan> Artisans { get; set; } = null!;

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}

