namespace GymClassBooking.Application.DTOs.Responses;

public sealed record GymClassResponse(
    Guid Id,
    string Type,
    DateTime ScheduledAt,
    int MaxCapacity,
    int AvailableSpots
);