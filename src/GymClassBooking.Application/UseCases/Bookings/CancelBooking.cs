using GymClassBooking.Application.Interfaces;
using GymClassBooking.Domain.Common;
using GymClassBooking.Domain.Errors;

namespace GymClassBooking.Application.UseCases.Bookings;

public sealed class CancelBooking(
    IBookingRepository bookingRepository,
    IGymClassRepository gymClassRepository,
    IUnitOfWork unitOfWork,
    IDateTimeProvider dateTimeProvider)
{
    public async Task<Result> ExecuteAsync(Guid bookingId, CancellationToken ct = default)
    {
        var booking = await bookingRepository.GetByIdAsync(bookingId, ct);
        if (booking is null)
            return Result.Failure(DomainErrors.Booking.NotFound);

        var gymClass = await gymClassRepository.GetByIdAsync(booking.GymClassId, ct);
        if (gymClass is null)
            return Result.Failure(DomainErrors.GymClass.NotFound);

        if (gymClass.ScheduledAt <= dateTimeProvider.UtcNow)
            return Result.Failure(DomainErrors.Booking.ClassAlreadyStarted);

        gymClass.Unenroll();

        await bookingRepository.RemoveAsync(booking, ct);
        await unitOfWork.CommitAsync(ct);

        return Result.Success();
    }
}