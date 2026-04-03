using GymClassBooking.Application.DTOs.Requests;
using GymClassBooking.Application.UseCases.GymClasses;
using GymClassBooking.Api.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace GymClassBooking.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class GymClassesController(
    CreateGymClass createGymClass,
    GetAllGymClasses getAllGymClasses) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct) =>
        Ok(await getAllGymClasses.ExecuteAsync(ct));

    [HttpPost]
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