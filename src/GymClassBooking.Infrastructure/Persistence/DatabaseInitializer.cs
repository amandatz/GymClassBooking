using GymClassBooking.Domain.Entities;
using GymClassBooking.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace GymClassBooking.Infrastructure.Persistence;

public static class DatabaseInitializer
{
    public static void Seed(AppDbContext context)
    {
        if (context.Students.Any()) return;

        var students = CreateStudents();
        var classes = CreateGymClasses();

        context.Students.AddRange(students);
        context.GymClasses.AddRange(classes);
        context.SaveChanges();

        SeedBookings(context, students, classes);
        context.SaveChanges();
    }

    public static async Task SeedAsync(AppDbContext context, CancellationToken ct = default)
    {
        if (await context.Students.AnyAsync(cancellationToken: ct)) return;

        var students = CreateStudents();
        var classes = CreateGymClasses();

        await context.Students.AddRangeAsync(students, ct);
        await context.GymClasses.AddRangeAsync(classes, ct);
        await context.SaveChangesAsync(ct);

        SeedBookings(context, students, classes);
        await context.SaveChangesAsync(ct);
    }

    private static List<Student> CreateStudents() =>
    [
        Student.Create("Alice Silva", "alice@silva.com", PlanType.Monthly),
        Student.Create("Bernardo Oliveira",  "bernardo@oliveira.com",   PlanType.Quarterly),
        Student.Create("Carol Santos",   "carol@santos.com", PlanType.Annual),
    ];

    private static List<GymClass> CreateGymClasses()
    {
        var tomorrow = DateTime.UtcNow.AddDays(1).Date;
        var dayAfter = DateTime.UtcNow.AddDays(2).Date;

        var results = new[]
        {
            GymClass.Create(ClassType.Cross,      tomorrow.AddHours(7),  20),
            GymClass.Create(ClassType.Pilates,    tomorrow.AddHours(9),  10),
            GymClass.Create(ClassType.Functional, dayAfter.AddHours(18), 15),
            GymClass.Create(ClassType.Yoga,       dayAfter.AddHours(8),  12),
        };

        if (results.Any(r => r.IsFailure))
            throw new InvalidOperationException("Failed to create seed gym classes. Check scheduled dates.");

        return [.. results.Select(r => r.Value)];
    }

    private static void SeedBookings(AppDbContext context, List<Student> students, List<GymClass> classes)
    {
        var alice = students[0];
        var bob = students[1];

        var cross = classes[0];
        var pilates = classes[1];
        var functional = classes[2];
        var yoga = classes[3];

        var bookings = new[]
        {
            Booking.Create(alice.Id, cross.Id),
            Booking.Create(alice.Id, pilates.Id),
            Booking.Create(bob.Id,   cross.Id),
            Booking.Create(bob.Id,   functional.Id),
            Booking.Create(bob.Id,   yoga.Id),
        };

        foreach (var booking in bookings)
        {
            var gymClass = classes.First(c => c.Id == booking.GymClassId);
            gymClass.Enroll();
        }

        context.Bookings.AddRange(bookings);
    }
}