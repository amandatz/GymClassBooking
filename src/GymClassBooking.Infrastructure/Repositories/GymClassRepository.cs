using GymClassBooking.Application.Interfaces;
using GymClassBooking.Domain.Entities;
using GymClassBooking.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GymClassBooking.Infrastructure.Repositories;

public sealed class GymClassRepository(AppDbContext context) : IGymClassRepository
{
    public async Task<GymClass?> GetByIdAsync(Guid id, CancellationToken ct = default) =>
        await context.GymClasses.FindAsync([id], ct);

    public async Task<IReadOnlyList<GymClass>> GetAllAsync(CancellationToken ct = default) =>
        await context.GymClasses.ToListAsync(ct);

    public async Task AddAsync(GymClass gymClass, CancellationToken ct = default) =>
        await context.GymClasses.AddAsync(gymClass, ct);
}