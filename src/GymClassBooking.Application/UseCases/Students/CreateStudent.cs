using GymClassBooking.Application.DTOs.Requests;
using GymClassBooking.Application.DTOs.Responses;
using GymClassBooking.Application.Interfaces;
using GymClassBooking.Domain.Common;
using GymClassBooking.Domain.Entities;
using GymClassBooking.Domain.Enums;
using GymClassBooking.Domain.Errors;

namespace GymClassBooking.Application.UseCases.Students;

public sealed class CreateStudent(
    IStudentRepository studentRepository,
    IUnitOfWork unitOfWork)
{
    public async Task<Result<StudentResponse>> ExecuteAsync(
        CreateStudentRequest request,
        CancellationToken ct = default)
    {
        var emailExists = await studentRepository.GetByEmailAsync(request.Email, ct);
        if (emailExists is not null)
            return Result.Failure<StudentResponse>(DomainErrors.Student.EmailAlreadyExists);

        var plan = PlanType.FromId(request.PlanId);
        if (plan is null)
            return Result.Failure<StudentResponse>(DomainErrors.Student.InvalidPlan);

        var student = Student.Create(request.Name, request.Email, plan);

        await studentRepository.AddAsync(student, ct);
        await unitOfWork.CommitAsync(ct);

        return new StudentResponse(
            student.Id,
            student.Name,
            student.Email,
            student.Plan.Name,
            student.CreatedAt);
    }
}