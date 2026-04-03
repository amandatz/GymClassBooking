namespace GymClassBooking.Application.DTOs.Responses;

public sealed record StudentReportResponse(
    Guid StudentId,
    string Name,
    string Plan,
    int Month,
    int Year,
    int TotalBookings,
    int PlanLimit,
    IReadOnlyList<ClassFrequencyResponse> MostFrequentClasses
);

public sealed record ClassFrequencyResponse(
    string Type,
    int Count
);