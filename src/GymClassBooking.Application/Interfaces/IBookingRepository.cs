using GymClassBooking.Domain.Entities;

namespace GymClassBooking.Application.Interfaces;

public interface IBookingRepository
{
    Task<Booking?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<IReadOnlyList<Booking>> GetAllAsync(CancellationToken ct = default);
    Task<bool> ExistsAsync(Guid studentId, Guid gymClassId, CancellationToken ct = default);
    Task<int> CountByStudentAndMonthAsync(Guid studentId, int month, int year, CancellationToken ct = default);
    Task AddAsync(Booking booking, CancellationToken ct = default);
    Task RemoveAsync(Booking booking, CancellationToken ct = default);
}