namespace GymClassBooking.Domain.Common;

public record Error(string Code, string Message)
{
    public static readonly Error None = new(string.Empty, string.Empty);
}

public record NotFoundError(string Code, string Message) : Error(Code, Message);
public record ValidationError(string Code, string Message) : Error(Code, Message);
public record ConflictError(string Code, string Message) : Error(Code, Message);