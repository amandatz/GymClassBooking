using GymClassBooking.Application.DTOs.Requests;
using GymClassBooking.Application.UseCases.Bookings;
using GymClassBooking.Api.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace GymClassBooking.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class BookingsController(
    CreateBooking createBooking,
    CancelBooking cancelBooking,
    GetAllBookings getAllBookings) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct) =>
        Ok(await getAllBookings.ExecuteAsync(ct));

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateBookingRequest request,
        CancellationToken ct)
    {
        var result = await createBooking.ExecuteAsync(request, ct);
        return result.IsSuccess
            ? CreatedAtAction(nameof(GetAll), new { id = result.Value.Id }, result.Value)
            : result.ToActionResult();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Cancel(Guid id, CancellationToken ct)
    {
        var result = await cancelBooking.ExecuteAsync(id, ct);
        return result.ToActionResult();
    }
}