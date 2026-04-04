using GymClassBooking.Api.Extensions;
using GymClassBooking.Application.DTOs.Responses;
using GymClassBooking.Application.UseCases.Reports;
using Microsoft.AspNetCore.Mvc;

namespace GymClassBooking.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class ReportsController(GetStudentReport getStudentReport) : ControllerBase
{
    /// <summary>Returns the monthly booking report for a student.</summary>
    /// <remarks>
    /// Returns total bookings for the month, plan limit and most frequent class types.
    /// Returns 404 if student is not found.
    /// </remarks>
    [HttpGet("students/{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(StudentReportResponse))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
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