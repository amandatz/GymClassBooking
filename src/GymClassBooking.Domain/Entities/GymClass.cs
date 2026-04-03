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
        CurrentEnrollment = 0;
        CreatedAt = DateTime.UtcNow;
    }

    public ClassType Type { get; private set; }
    public DateTime ScheduledAt { get; private set; }
    public int MaxCapacity { get; private set; }
    public int CurrentEnrollment { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public int AvailableSpots => MaxCapacity - CurrentEnrollment;
    public bool IsFull => CurrentEnrollment >= MaxCapacity;

    public static Result<GymClass> Create(ClassType type, DateTime scheduledAt, int maxCapacity)
    {
        if (scheduledAt <= DateTime.UtcNow)
            return Result.Failure<GymClass>(DomainErrors.GymClass.ScheduledInThePast);

        return new GymClass(Guid.NewGuid(), type, scheduledAt, maxCapacity);
    }

    public Result Enroll()
    {
        if (IsFull)
            return Result.Failure(DomainErrors.GymClass.AtFullCapacity);

        CurrentEnrollment++;
        return Result.Success();
    }

    public void Unenroll()
    {
        if (CurrentEnrollment > 0)
            CurrentEnrollment--;
    }
}