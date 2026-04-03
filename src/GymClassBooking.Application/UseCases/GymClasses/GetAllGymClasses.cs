using GymClassBooking.Application.DTOs.Responses;
using GymClassBooking.Application.Interfaces;

namespace GymClassBooking.Application.UseCases.GymClasses;

public sealed class GetAllGymClasses(IGymClassRepository gymClassRepository)
{
    public async Task<IReadOnlyList<GymClassResponse>> ExecuteAsync(CancellationToken ct = default)
    {
        var classes = await gymClassRepository.GetAllAsync(ct);

        return [.. classes.Select(c => new GymClassResponse(
            c.Id,
            c.Type.ToString(),
            c.ScheduledAt,
            c.MaxCapacity,
            c.MaxCapacity))];
    }
}