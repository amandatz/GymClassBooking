using GymClassBooking.Domain.Common;
using GymClassBooking.Domain.Errors;

namespace GymClassBooking.Domain.Entities;

public sealed class Student : Entity
{
    private readonly List<Booking> _bookings = [];

    private Student() { }

    private Student(Guid id, string name, string email, PlanType plan) : base(id)
    {
        Name = name;
        Email = email;
        Plan = plan;
        CreatedAt = DateTime.UtcNow;
    }

    public string Name { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public PlanType Plan { get; private set; } = null!;
    public DateTime CreatedAt { get; private set; }

    public IReadOnlyCollection<Booking> Bookings => _bookings.AsReadOnly();

    public static Student Create(string name, string email, PlanType plan) =>
        new(Guid.NewGuid(), name.Trim(), email.Trim().ToLowerInvariant(), plan);

    public Result CanBook(int bookingsThisMonth)
    {
        return bookingsThisMonth >= Plan.MonthlyLimit
            ? Result.Failure(DomainErrors.Booking.PlanLimitReached)
            : Result.Success();
    }
}