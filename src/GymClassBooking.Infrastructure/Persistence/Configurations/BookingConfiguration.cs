using GymClassBooking.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymClassBooking.Infrastructure.Persistence.Configurations;

public sealed class BookingConfiguration : IEntityTypeConfiguration<Booking>
{
    public void Configure(EntityTypeBuilder<Booking> builder)
    {
        builder.HasKey(b => b.Id);

        builder.HasIndex(b => new { b.StudentId, b.GymClassId })
            .IsUnique();

        builder.HasOne(b => b.Student)
            .WithMany(s => s.Bookings)
            .HasForeignKey(b => b.StudentId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(b => b.GymClass)
            .WithMany()
            .HasForeignKey(b => b.GymClassId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.ToTable("Bookings");
    }
}