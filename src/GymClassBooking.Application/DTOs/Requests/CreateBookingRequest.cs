namespace GymClassBooking.Application.DTOs.Requests;

public sealed record CreateBookingRequest(
    Guid StudentId,
    Guid GymClassId
);