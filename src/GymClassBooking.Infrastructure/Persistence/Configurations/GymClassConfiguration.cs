using GymClassBooking.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymClassBooking.Infrastructure.Persistence.Configurations;

public sealed class GymClassConfiguration : IEntityTypeConfiguration<GymClass>
{
    public void Configure(EntityTypeBuilder<GymClass> builder)
    {
        builder.HasKey(g => g.Id);

        builder.Property(g => g.Type)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(g => g.MaxCapacity)
            .IsRequired();

        builder.Property(g => g.CurrentEnrollment)
            .IsConcurrencyToken();

        builder.ToTable("GymClasses");
    }
}