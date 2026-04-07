using GymClassBooking.Api.Extensions;
using GymClassBooking.Application.DTOs.Requests;
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
    /// Returns 400 if month or year are invalid.
    /// </remarks>
    [HttpGet("students/{id:guid}")]
    public async Task<IActionResult> GetStudentReport(
    Guid id,
    [FromQuery] StudentReportRequest request,
    CancellationToken ct)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await getStudentReport.ExecuteAsync(id, request.Month!.Value, request.Year!.Value, ct);
        return result.IsSuccess
            ? Ok(result.Value)
            : result.ToActionResult();
    }
}