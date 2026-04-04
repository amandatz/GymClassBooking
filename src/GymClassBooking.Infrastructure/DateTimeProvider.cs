using GymClassBooking.Application.Interfaces;

namespace GymClassBooking.Infrastructure;

public sealed class DateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
}