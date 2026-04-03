namespace GymClassBooking.Application.DTOs.Responses;

public sealed record BookingResponse(
    Guid Id,
    Guid StudentId,
    string StudentName,
    Guid GymClassId,
    string GymClassType,
    DateTime ScheduledAt,
    DateTime BookedAt
);