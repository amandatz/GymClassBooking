using GymClassBooking.Application.DTOs.Responses;
using GymClassBooking.Application.Interfaces;

namespace GymClassBooking.Application.UseCases.Bookings;

public sealed class GetAllBookings(IBookingRepository bookingRepository)
{
    public async Task<IReadOnlyList<BookingResponse>> ExecuteAsync(CancellationToken ct = default)
    {
        return await bookingRepository.GetAllProjectedAsync(ct);
    }
}