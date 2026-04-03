using GymClassBooking.Application.DTOs.Requests;
using GymClassBooking.Application.UseCases.Students;
using GymClassBooking.Api.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace GymClassBooking.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class StudentsController(
    CreateStudent createStudent,
    GetAllStudents getAllStudents) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct) =>
        Ok(await getAllStudents.ExecuteAsync(ct));

    [HttpPost]
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