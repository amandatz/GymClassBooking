using GymClassBooking.Application.UseCases.Reports;
using GymClassBooking.Api.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace GymClassBooking.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class ReportsController(GetStudentReport getStudentReport) : ControllerBase
{
    [HttpGet("students/{id:guid}")]
    public async Task<IActionResult> GetStudentReport(
        Guid id,
        [FromQuery] int month,
        [FromQuery] int year,
        CancellationToken ct)
    {
        var result = await getStudentReport.ExecuteAsync(id, month, year, ct);
        return result.IsSuccess
            ? Ok(result.Value)
            : result.ToActionResult();
    }
}