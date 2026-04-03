using GymClassBooking.Domain.Entities;

namespace GymClassBooking.Application.Interfaces;

public interface IGymClassRepository
{
    Task<GymClass?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<IReadOnlyList<GymClass>> GetAllAsync(CancellationToken ct = default);
    Task AddAsync(GymClass gymClass, CancellationToken ct = default);
}