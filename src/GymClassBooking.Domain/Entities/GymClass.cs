using GymClassBooking.Domain.Common;
using GymClassBooking.Domain.Enums;
using GymClassBooking.Domain.Errors;

namespace GymClassBooking.Domain.Entities;

public sealed class GymClass : Entity
{
    private GymClass() { }

    private GymClass(Guid id, ClassType type, DateTime scheduledAt, int maxCapacity) : base(id)
    {
        Type = type;
        ScheduledAt = scheduledAt;
        MaxCapacity = maxCapacity;
        CreatedAt = DateTime.UtcNow;
    }

    public ClassType Type { get; private set; }
    public DateTime ScheduledAt { get; private set; }
    public int MaxCapacity { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public static Result<GymClass> Create(ClassType type, DateTime scheduledAt, int maxCapacity)
    {
        if (scheduledAt <= DateTime.UtcNow)
            return DomainErrors.GymClass.ScheduledInThePast;

        return new GymClass(Guid.NewGuid(), type, scheduledAt, maxCapacity);
    }
}