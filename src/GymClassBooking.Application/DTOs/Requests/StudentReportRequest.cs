using System.ComponentModel.DataAnnotations;

namespace GymClassBooking.Application.DTOs.Requests;

public sealed record StudentReportRequest
{
    [Required]
    [Range(1, 12, ErrorMessage = "Month must be between 1 and 12.")]
    public int? Month { get; init; }

    [Required]
    [Range(1900, 2100, ErrorMessage = "Year must be between 1900 and 2100.")]
    public int? Year { get; init; }
}
