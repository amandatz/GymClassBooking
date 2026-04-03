using FluentAssertions;
using GymClassBooking.Domain.Entities;
using GymClassBooking.Domain.Enums;
using GymClassBooking.Domain.Errors;

namespace GymClassBooking.UnitTests.Domain;

public sealed class StudentTests
{
    [Fact]
    public void CanBook_WhenUnderMonthlyLimit_ShouldSucceed()
    {
        // Arrange
        var student = Student.Create("Anna", "anna@test.com", PlanType.Monthly);

        // Act
        var result = student.CanBook(bookingsThisMonth: 0);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    [Theory]
    [InlineData(12, 12)]
    [InlineData(20, 20)]
    [InlineData(30, 30)]
    public void CanBook_WhenAtPlanLimit_ShouldFail(int limit, int bookingsThisMonth)
    {
        // Arrange
        var plan = limit switch
        {
            12 => PlanType.Monthly,
            20 => PlanType.Quarterly,
            _ => PlanType.Annual
        };

        var student = Student.Create("Test", "test@test.com", plan);

        // Act
        var result = student.CanBook(bookingsThisMonth);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(DomainErrors.Booking.PlanLimitReached);
    }

    [Theory]
    [InlineData(12, 11)]
    [InlineData(20, 19)]
    [InlineData(30, 29)]
    public void CanBook_WhenOneSlotLeft_ShouldSucceed(int limit, int bookingsThisMonth)
    {
        // Arrange
        var plan = limit switch
        {
            12 => PlanType.Monthly,
            20 => PlanType.Quarterly,
            _ => PlanType.Annual
        };

        var student = Student.Create("Test", "test@test.com", plan);

        // Act
        var result = student.CanBook(bookingsThisMonth);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }}