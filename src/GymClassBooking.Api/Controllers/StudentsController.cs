using GymClassBooking.Api.Extensions;
using GymClassBooking.Application.DTOs.Requests;
using GymClassBooking.Application.DTOs.Responses;
using GymClassBooking.Application.UseCases.Students;
using Microsoft.AspNetCore.Mvc;

namespace GymClassBooking.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class StudentsController(
    CreateStudent createStudent,
    GetAllStudents getAllStudents) : ControllerBase
{
    /// <summary>Lists all registered students.</summary>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IReadOnlyList<StudentResponse>))]
    public async Task<IActionResult> GetAll(CancellationToken ct) =>
        Ok(await getAllStudents.ExecuteAsync(ct));

    /// <summary>Creates a new student.</summary>
    /// <remarks>
    /// Returns 409 if email is already registered.
    /// Returns 400 if plan ID is invalid.
    /// </remarks>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(StudentResponse))]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(
        [FromBody] CreateStudentRequest request,
        CancellationToken ct)
    {
        var result = await createStudent.ExecuteAsync(request, ct);
        return result.IsSuccess
            ? CreatedAtAction(nameof(GetAll), new { id = result.Value.Id }, result.Value)
            : result.ToActionResult();
    }
}