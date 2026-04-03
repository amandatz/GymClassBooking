namespace GymClassBooking.Application.DTOs.Requests;

public sealed record CreateGymClassRequest(
    int ClassTypeId,
    DateTime ScheduledAt,
    int MaxCapacity
);