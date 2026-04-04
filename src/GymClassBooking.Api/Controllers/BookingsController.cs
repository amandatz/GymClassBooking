using GymClassBooking.Api.Extensions;
using GymClassBooking.Application.DTOs.Requests;
using GymClassBooking.Application.DTOs.Responses;
using GymClassBooking.Application.UseCases.Bookings;
using Microsoft.AspNetCore.Mvc;

namespace GymClassBooking.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class BookingsController(
    CreateBooking createBooking,
    CancelBooking cancelBooking,
    GetAllBookings getAllBookings) : ControllerBase
{
    /// <summary>Lists all bookings.</summary>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IReadOnlyList<BookingResponse>))]
    public async Task<IActionResult> GetAll(CancellationToken ct) =>
        Ok(await getAllBookings.ExecuteAsync(ct));

    /// <summary>Books a student into a gym class.</summary>
    /// <remarks>
    /// Returns 404 if student or class is not found.
    /// Returns 409 if the class is at full capacity or student is already booked.
    /// Returns 400 if the student has reached their monthly plan limit.
    /// </remarks>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(BookingResponse))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(
        [FromBody] CreateBookingRequest request,
        CancellationToken ct)
    {
        var result = await createBooking.ExecuteAsync(request, ct);
        return result.IsSuccess
            ? CreatedAtAction(nameof(GetAll), new { id = result.Value.Id }, result.Value)
            : result.ToActionResult();
    }

    /// <summary>Cancels an existing booking.</summary>
    /// <remarks>
    /// Returns 404 if booking is not found.
    /// Returns 400 if the class starts in less than 2 hours.
    /// </remarks>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Cancel(Guid id, CancellationToken ct)
    {
        var result = await cancelBooking.ExecuteAsync(id, ct);
        return result.ToActionResult();
    }
}