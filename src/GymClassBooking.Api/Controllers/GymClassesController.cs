using GymClassBooking.Api.Extensions;
using GymClassBooking.Application.DTOs.Requests;
using GymClassBooking.Application.DTOs.Responses;
using GymClassBooking.Application.UseCases.GymClasses;
using Microsoft.AspNetCore.Mvc;

namespace GymClassBooking.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class GymClassesController(
    CreateGymClass createGymClass,
    GetAllGymClasses getAllGymClasses) : ControllerBase
{
    /// <summary>Lists all gym classes.</summary>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IReadOnlyList<GymClassResponse>))]
    public async Task<IActionResult> GetAll(CancellationToken ct) =>
        Ok(await getAllGymClasses.ExecuteAsync(ct));

    /// <summary>Creates a new gym class.</summary>
    /// <remarks>
    /// Returns 400 if scheduled date is in the past or class type is invalid.
    /// </remarks>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(GymClassResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(
        [FromBody] CreateGymClassRequest request,
        CancellationToken ct)
    {
        var result = await createGymClass.ExecuteAsync(request, ct);
        return result.IsSuccess
            ? CreatedAtAction(nameof(GetAll), new { id = result.Value.Id }, result.Value)
            : result.ToActionResult();
    }
}