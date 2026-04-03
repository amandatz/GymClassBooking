namespace GymClassBooking.Domain.Common.Exceptions;

public sealed class ConcurrencyException : Exception
{
    public ConcurrencyException()
        : base("The resource was modified by another request. Please try again.") { }
}