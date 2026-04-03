using GymClassBooking.Domain.Common;

namespace GymClassBooking.Domain.Entities;

public sealed class Booking : Entity
{
    private Booking() { }

    private Booking(Guid id, Guid studentId, Guid gymClassId) : base(id)
    {
        StudentId = studentId;
        GymClassId = gymClassId;
        BookedAt = DateTime.UtcNow;
    }

    public Guid StudentId { get; private set; }
    public Guid GymClassId { get; private set; }
    public DateTime BookedAt { get; private set; }

    public Student Student { get; private set; } = null!;
    public GymClass GymClass { get; private set; } = null!;

    public static Booking Create(Guid studentId, Guid gymClassId) =>
        new(Guid.NewGuid(), studentId, gymClassId);
}