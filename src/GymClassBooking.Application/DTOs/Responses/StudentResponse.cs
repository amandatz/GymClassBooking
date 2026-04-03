namespace GymClassBooking.Application.DTOs.Responses;

public sealed record StudentResponse(
    Guid Id,
    string Name,
    string Email,
    string Plan,
    DateTime CreatedAt
);