using GymClassBooking.Application.DTOs.Responses;
using GymClassBooking.Application.Interfaces;
using GymClassBooking.Domain.Common;
using GymClassBooking.Domain.Errors;

namespace GymClassBooking.Application.UseCases.Reports;

public sealed class GetStudentReport(
    IStudentRepository studentRepository,
    IBookingRepository bookingRepository)
{
    public async Task<Result<StudentReportResponse>> ExecuteAsync(
        Guid studentId,
        int month,
        int year,
        CancellationToken ct = default)
    {
        var student = await studentRepository.GetByIdAsync(studentId, ct);
        if (student is null)
            return Result.Failure<StudentReportResponse>(DomainErrors.Student.NotFound);

        var bookingsThisMonth = await bookingRepository
            .GetByStudentAndMonthAsync(studentId, month, year, ct);

        var mostFrequent = bookingsThisMonth
            .GroupBy(b => b.GymClass.Type.ToString())
            .OrderByDescending(g => g.Count())
            .Select(g => new ClassFrequencyResponse(g.Key, g.Count()))
            .ToList();

        return new StudentReportResponse(
            student.Id,
            student.Name,
            student.Plan.Name,
            month,
            year,
            bookingsThisMonth.Count,
            student.Plan.MonthlyLimit,
            mostFrequent);
    }
}