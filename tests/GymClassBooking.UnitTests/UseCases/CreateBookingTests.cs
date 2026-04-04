using FluentAssertions;
using GymClassBooking.Application.DTOs.Requests;
using GymClassBooking.Application.Interfaces;
using GymClassBooking.Application.UseCases.Bookings;
using GymClassBooking.Domain.Entities;
using GymClassBooking.Domain.Enums;
using GymClassBooking.Domain.Errors;
using NSubstitute;

namespace GymClassBooking.UnitTests.UseCases;

public sealed class CreateBookingTests
{
    private readonly IStudentRepository _studentRepository = Substitute.For<IStudentRepository>();
    private readonly IGymClassRepository _gymClassRepository = Substitute.For<IGymClassRepository>();
    private readonly IBookingRepository _bookingRepository = Substitute.For<IBookingRepository>();
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
    private readonly CreateBooking _sut;

    public CreateBookingTests()
    {
        _sut = new CreateBooking(
            _studentRepository,
            _gymClassRepository,
            _bookingRepository,
            _unitOfWork);
    }

    [Fact]
    public async Task Execute_WhenStudentNotFound_ShouldReturnNotFound()
    {
        // Arrange
        var request = new CreateBookingRequest(Guid.NewGuid(), Guid.NewGuid());
        _studentRepository.GetByIdAsync(request.StudentId).Returns((Student?)null);

        // Act
        var result = await _sut.ExecuteAsync(request);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(DomainErrors.Student.NotFound);
    }

    [Fact]
    public async Task Execute_WhenClassNotFound_ShouldReturnNotFound()
    {
        // Arrange
        var student = Student.Create("Alice", "alice@test.com", PlanType.Monthly);
        var request = new CreateBookingRequest(student.Id, Guid.NewGuid());

        _studentRepository.GetByIdAsync(request.StudentId).Returns(student);
        _gymClassRepository.GetByIdAsync(request.GymClassId).Returns((GymClass?)null);

        // Act
        var result = await _sut.ExecuteAsync(request);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(DomainErrors.GymClass.NotFound);
    }

    [Fact]
    public async Task Execute_WhenAlreadyBooked_ShouldReturnConflict()
    {
        // Arrange
        var student = Student.Create("Alice", "alice@test.com", PlanType.Monthly);
        var gymClass = GymClass.Create(ClassType.Cross, DateTime.UtcNow.AddDays(1), 20).Value;
        var request = new CreateBookingRequest(student.Id, gymClass.Id);

        _studentRepository.GetByIdAsync(request.StudentId).Returns(student);
        _gymClassRepository.GetByIdAsync(request.GymClassId).Returns(gymClass);
        _bookingRepository.ExistsAsync(request.StudentId, request.GymClassId).Returns(true);

        // Act
        var result = await _sut.ExecuteAsync(request);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(DomainErrors.Booking.AlreadyExists);
    }

    [Fact]
    public async Task Execute_WhenPlanLimitReached_ShouldReturnValidationError()
    {
        // Arrange
        var student = Student.Create("Alice", "alice@test.com", PlanType.Monthly);
        var gymClass = GymClass.Create(ClassType.Cross, DateTime.UtcNow.AddDays(1), 20).Value;
        var request = new CreateBookingRequest(student.Id, gymClass.Id);

        _studentRepository.GetByIdAsync(request.StudentId).Returns(student);
        _gymClassRepository.GetByIdAsync(request.GymClassId).Returns(gymClass);
        _bookingRepository.ExistsAsync(request.StudentId, request.GymClassId).Returns(false);
        _bookingRepository
            .CountByStudentAndMonthAsync(request.StudentId, gymClass.ScheduledAt.Month, gymClass.ScheduledAt.Year)
            .Returns(12);

        // Act
        var result = await _sut.ExecuteAsync(request);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(DomainErrors.Booking.PlanLimitReached);
    }

    [Fact]
    public async Task Execute_WhenClassAtFullCapacity_ShouldReturnConflict()
    {
        // Arrange
        var student = Student.Create("Alice", "alice@test.com", PlanType.Monthly);
        var gymClass = GymClass.Create(ClassType.Cross, DateTime.UtcNow.AddDays(1), 1).Value;
        gymClass.Enroll();
        var request = new CreateBookingRequest(student.Id, gymClass.Id);

        _studentRepository.GetByIdAsync(request.StudentId).Returns(student);
        _gymClassRepository.GetByIdAsync(request.GymClassId).Returns(gymClass);
        _bookingRepository.ExistsAsync(request.StudentId, request.GymClassId).Returns(false);
        _bookingRepository
            .CountByStudentAndMonthAsync(request.StudentId, gymClass.ScheduledAt.Month, gymClass.ScheduledAt.Year)
            .Returns(0);

        // Act
        var result = await _sut.ExecuteAsync(request);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(DomainErrors.GymClass.AtFullCapacity);
    }

    [Fact]
    public async Task Execute_WhenValid_ShouldSucceed()
    {
        // Arrange
        var student = Student.Create("Alice", "alice@test.com", PlanType.Monthly);
        var gymClass = GymClass.Create(ClassType.Cross, DateTime.UtcNow.AddDays(1), 20).Value;
        var request = new CreateBookingRequest(student.Id, gymClass.Id);

        _studentRepository.GetByIdAsync(request.StudentId).Returns(student);
        _gymClassRepository.GetByIdAsync(request.GymClassId).Returns(gymClass);
        _bookingRepository.ExistsAsync(request.StudentId, request.GymClassId).Returns(false);
        _bookingRepository
            .CountByStudentAndMonthAsync(request.StudentId, gymClass.ScheduledAt.Month, gymClass.ScheduledAt.Year)
            .Returns(0);

        // Act
        var result = await _sut.ExecuteAsync(request);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.StudentId.Should().Be(student.Id);
        result.Value.GymClassId.Should().Be(gymClass.Id);
    }
}