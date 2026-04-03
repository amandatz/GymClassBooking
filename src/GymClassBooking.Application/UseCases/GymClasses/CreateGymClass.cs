using GymClassBooking.Application.DTOs.Requests;
using GymClassBooking.Application.DTOs.Responses;
using GymClassBooking.Application.Interfaces;
using GymClassBooking.Domain.Common;
using GymClassBooking.Domain.Enums;
using GymClassBooking.Domain.Errors;

namespace GymClassBooking.Application.UseCases.GymClasses;

public sealed class CreateGymClass(
    IGymClassRepository gymClassRepository,
    IUnitOfWork unitOfWork)
{
    public async Task<Result<GymClassResponse>> ExecuteAsync(
        CreateGymClassRequest request,
        CancellationToken ct = default)
    {
        if (!Enum.IsDefined(typeof(ClassType), request.ClassTypeId))
            return Result.Failure<GymClassResponse>(DomainErrors.GymClass.InvalidClassType);

        var classType = (ClassType)request.ClassTypeId;

        var result = Domain.Entities.GymClass.Create(classType, request.ScheduledAt, request.MaxCapacity);
        if (result.IsFailure)
            return Result.Failure<GymClassResponse>(result.Error);

        await gymClassRepository.AddAsync(result.Value, ct);
        await unitOfWork.CommitAsync(ct);

        var gymClass = result.Value;

        return new GymClassResponse(
            gymClass.Id,
            gymClass.Type.ToString(),
            gymClass.ScheduledAt,
            gymClass.MaxCapacity,
            gymClass.MaxCapacity);
    }
}