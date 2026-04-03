using FluentAssertions;
using GymClassBooking.Domain.Entities;
using GymClassBooking.Domain.Enums;
using GymClassBooking.Domain.Errors;

namespace GymClassBooking.UnitTests.Domain;

public sealed class GymClassTests
{
    [Fact]
    public void Create_WhenScheduledInThePast_ShouldFail()
    {
        // Arrange
        var pastDate = DateTime.UtcNow.AddDays(-1);

        // Act
        var result = GymClass.Create(ClassType.Cross, pastDate, 20);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(DomainErrors.GymClass.ScheduledInThePast);
    }

    [Fact]
    public void Create_WhenScheduledInTheFuture_ShouldSucceed()
    {
        // Arrange
        var futureDate = DateTime.UtcNow.AddDays(1);

        // Act
        var result = GymClass.Create(ClassType.Cross, futureDate, 20);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.AvailableSpots.Should().Be(20);
    }

    [Fact]
    public void Enroll_WhenClassIsFull_ShouldFail()
    {
        // Arrange
        var gymClass = GymClass.Create(ClassType.Cross, DateTime.UtcNow.AddDays(1), 1).Value;
        gymClass.Enroll();

        // Act
        var result = gymClass.Enroll();

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(DomainErrors.GymClass.AtFullCapacity);
    }

    [Fact]
    public void Enroll_WhenClassHasSpots_ShouldSucceedAndDecrementSpots()
    {
        // Arrange
        var gymClass = GymClass.Create(ClassType.Pilates, DateTime.UtcNow.AddDays(1), 10).Value;

        // Act
        var result = gymClass.Enroll();

        // Assert
        result.IsSuccess.Should().BeTrue();
        gymClass.AvailableSpots.Should().Be(9);
        gymClass.CurrentEnrollment.Should().Be(1);
    }

    [Fact]
    public void Unenroll_ShouldDecrementEnrollment()
    {
        // Arrange
        var gymClass = GymClass.Create(ClassType.Yoga, DateTime.UtcNow.AddDays(1), 10).Value;
        gymClass.Enroll();

        // Act
        gymClass.Unenroll();

        // Assert
        gymClass.CurrentEnrollment.Should().Be(0);
        gymClass.AvailableSpots.Should().Be(10);
    }
}