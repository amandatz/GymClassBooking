using GymClassBooking.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GymClassBooking.Infrastructure.Persistence;

public sealed class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Student> Students { get; init; }
    public DbSet<GymClass> GymClasses { get; init; }
    public DbSet<Booking> Bookings { get; init; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        #if DEBUG
        optionsBuilder.EnableSensitiveDataLogging();
        #endif
    }
}