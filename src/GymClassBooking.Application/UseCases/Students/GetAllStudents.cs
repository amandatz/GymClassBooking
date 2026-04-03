using GymClassBooking.Application.DTOs.Responses;
using GymClassBooking.Application.Interfaces;

namespace GymClassBooking.Application.UseCases.Students;

public sealed class GetAllStudents(IStudentRepository studentRepository)
{
    public async Task<IReadOnlyList<StudentResponse>> ExecuteAsync(CancellationToken ct = default)
    {
        var students = await studentRepository.GetAllAsync(ct);

        return [.. students.Select(s => new StudentResponse(
            s.Id,
            s.Name,
            s.Email,
            s.Plan.Name,
            s.CreatedAt))];
    }
}