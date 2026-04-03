using GymClassBooking.Application.DTOs.Requests;
using GymClassBooking.Application.DTOs.Responses;
using GymClassBooking.Application.Interfaces;
using GymClassBooking.Domain.Common;
using GymClassBooking.Domain.Common.Exceptions;
using GymClassBooking.Domain.Entities;
using GymClassBooking.Domain.Errors;

namespace GymClassBooking.Application.UseCases.Bookings;

public sealed class CreateBooking(
    IStudentRepository studentRepository,
    IGymClassRepository gymClassRepository,
    IBookingRepository bookingRepository,
    IUnitOfWork unitOfWork)
{
    public async Task<Result<BookingResponse>> ExecuteAsync(
        CreateBookingRequest request,
        CancellationToken ct = default)
    {
        var student = await studentRepository.GetByIdAsync(request.StudentId, ct);
        if (student is null)
            return Result.Failure<BookingResponse>(DomainErrors.Student.NotFound);

        var gymClass = await gymClassRepository.GetByIdAsync(request.GymClassId, ct);
        if (gymClass is null)
            return Result.Failure<BookingResponse>(DomainErrors.GymClass.NotFound);

        var alreadyBooked = await bookingRepository.ExistsAsync(request.StudentId, request.GymClassId, ct);
        if (alreadyBooked)
            return Result.Failure<BookingResponse>(DomainErrors.Booking.AlreadyExists);

        var bookingsThisMonth = await bookingRepository.CountByStudentAndMonthAsync(
            request.StudentId,
            gymClass.ScheduledAt.Month,
            gymClass.ScheduledAt.Year,
            ct);

        var canBook = student.CanBook(bookingsThisMonth);
        if (canBook.IsFailure)
            return Result.Failure<BookingResponse>(canBook.Error);

        var enroll = gymClass.Enroll();
        if (enroll.IsFailure)
            return Result.Failure<BookingResponse>(enroll.Error);

        var booking = Booking.Create(request.StudentId, request.GymClassId);
        await bookingRepository.AddAsync(booking, ct);

        try
        {
            await unitOfWork.CommitAsync(ct);
        }
        catch (ConcurrencyException)
        {
            return Result.Failure<BookingResponse>(DomainErrors.GymClass.AtFullCapacity);
        }

        return new BookingResponse(
            booking.Id,
            student.Id,
            student.Name,
            gymClass.Id,
            gymClass.Type.ToString(),
            gymClass.ScheduledAt,
            booking.BookedAt);
    }
}