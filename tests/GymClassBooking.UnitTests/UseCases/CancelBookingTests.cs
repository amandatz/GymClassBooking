using FluentAssertions;
using GymClassBooking.Application.Interfaces;
using GymClassBooking.Application.UseCases.Bookings;
using GymClassBooking.Domain.Entities;
using GymClassBooking.Domain.Enums;
using GymClassBooking.Domain.Errors;
using NSubstitute;

namespace GymClassBooking.UnitTests.UseCases;

public sealed class CancelBookingTests
{
    private readonly IBookingRepository _bookingRepository = Substitute.For<IBookingRepository>();
    private readonly IGymClassRepository _gymClassRepository = Substitute.For<IGymClassRepository>();
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
    private readonly IDateTimeProvider _dateTimeProvider = Substitute.For<IDateTimeProvider>();
    private readonly CancelBooking _sut;

    public CancelBookingTests()
    {
        _dateTimeProvider.UtcNow.Returns(DateTime.UtcNow);

        _sut = new CancelBooking(
            _bookingRepository,
            _gymClassRepository,
            _unitOfWork,
            _dateTimeProvider);
    }

    [Fact]
    public async Task Execute_WhenBookingNotFound_ShouldReturnNotFound()
    {
        // Arrange
        _bookingRepository.GetByIdAsync(Arg.Any<Guid>()).Returns((Booking?)null);

        // Act
        var result = await _sut.ExecuteAsync(Guid.NewGuid());

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(DomainErrors.Booking.NotFound);
    }

    [Fact]
    public async Task Execute_WhenClassAlreadyStarted_ShouldReturnValidationError()
    {
        // Arrange
        var student = Student.Create("Alice", "alice@test.com", PlanType.Monthly);
        var gymClass = GymClass.Create(ClassType.Cross, DateTime.UtcNow.AddDays(1), 20).Value;
        var booking = Booking.Create(student.Id, gymClass.Id);

        _bookingRepository.GetByIdAsync(booking.Id).Returns(booking);
        _gymClassRepository.GetByIdAsync(booking.GymClassId).Returns(gymClass);

        _dateTimeProvider.UtcNow.Returns(gymClass.ScheduledAt.AddHours(1));

        // Act
        var result = await _sut.ExecuteAsync(booking.Id);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(DomainErrors.Booking.ClassAlreadyStarted);
    }

    [Fact]
    public async Task Execute_WhenValid_ShouldSucceed()
    {
        // Arrange
        var student = Student.Create("Alice", "alice@test.com", PlanType.Monthly);
        var gymClass = GymClass.Create(ClassType.Cross, DateTime.UtcNow.AddDays(1), 20).Value;
        var booking = Booking.Create(student.Id, gymClass.Id);

        _bookingRepository.GetByIdAsync(booking.Id).Returns(booking);
        _gymClassRepository.GetByIdAsync(booking.GymClassId).Returns(gymClass);
        _dateTimeProvider.UtcNow.Returns(DateTime.UtcNow);

        // Act
        var result = await _sut.ExecuteAsync(booking.Id);

        // Assert
        result.IsSuccess.Should().BeTrue();
        gymClass.CurrentEnrollment.Should().Be(0);
    }
}