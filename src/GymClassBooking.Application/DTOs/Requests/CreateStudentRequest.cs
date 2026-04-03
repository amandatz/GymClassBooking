namespace GymClassBooking.Application.DTOs.Requests;

public sealed record CreateStudentRequest(
    string Name,
    string Email,
    int PlanId
);