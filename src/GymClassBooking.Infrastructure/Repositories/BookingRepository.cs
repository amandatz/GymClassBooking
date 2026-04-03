using GymClassBooking.Application.DTOs.Responses;
using GymClassBooking.Application.Interfaces;
using GymClassBooking.Domain.Entities;
using GymClassBooking.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GymClassBooking.Infrastructure.Repositories;

public sealed class BookingRepository(AppDbContext context) : IBookingRepository
{
    public async Task<Booking?> GetByIdAsync(Guid id, CancellationToken ct = default) =>
        await context.Bookings
            .Include(b => b.Student)
            .Include(b => b.GymClass)
            .FirstOrDefaultAsync(b => b.Id == id, ct);

    public async Task<IReadOnlyList<Booking>> GetAllAsync(CancellationToken ct = default) =>
        await context.Bookings
            .Include(b => b.Student)
            .Include(b => b.GymClass)
            .ToListAsync(ct);

    public async Task<IReadOnlyList<BookingResponse>> GetAllProjectedAsync(CancellationToken ct = default) =>
        await context.Bookings
            .AsNoTracking()
            .Select(b => new BookingResponse(
                b.Id,
                b.StudentId,
                b.Student.Name,
                b.GymClassId,
                b.GymClass.Type.ToString(),
                b.GymClass.ScheduledAt,
                b.BookedAt))
            .ToListAsync(ct);

    public async Task<IReadOnlyList<Booking>> GetByStudentAndMonthAsync(
        Guid studentId, int month, int year, CancellationToken ct = default) =>
        await context.Bookings
            .Include(b => b.GymClass)
            .Where(b => b.StudentId == studentId
                     && b.GymClass.ScheduledAt.Month == month
                     && b.GymClass.ScheduledAt.Year == year)
            .ToListAsync(ct);

    public async Task<bool> ExistsAsync(Guid studentId, Guid gymClassId, CancellationToken ct = default) =>
        await context.Bookings
            .AnyAsync(b => b.StudentId == studentId
                        && b.GymClassId == gymClassId, ct);

    public async Task<int> CountByStudentAndMonthAsync(
        Guid studentId, int month, int year, CancellationToken ct = default) =>
        await context.Bookings
            .CountAsync(b => b.StudentId == studentId
                          && b.GymClass.ScheduledAt.Month == month
                          && b.GymClass.ScheduledAt.Year == year, ct);

    public async Task AddAsync(Booking booking, CancellationToken ct = default) =>
        await context.Bookings.AddAsync(booking, ct);

    public async Task RemoveAsync(Booking booking, CancellationToken ct = default) =>
        await Task.FromResult(context.Bookings.Remove(booking));
}