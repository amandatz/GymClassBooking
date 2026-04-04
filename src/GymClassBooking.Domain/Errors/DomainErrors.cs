using GymClassBooking.Domain.Common;

namespace GymClassBooking.Domain.Errors;

public static class DomainErrors
{
    public static class Student
    {
        public static readonly NotFoundError NotFound =
            new("Student.NotFound", "Student not found.");

        public static readonly ConflictError EmailAlreadyExists =
            new("Student.EmailAlreadyExists", "A student with this email already exists.");

        public static readonly ConflictError InvalidPlan =
            new("Student.InvalidPlan", "The provided plan is invalid.");
    }

    public static class GymClass
    {
        public static readonly NotFoundError NotFound =
            new("GymClass.NotFound", "Gym class not found.");

        public static readonly ConflictError AtFullCapacity =
            new("GymClass.AtFullCapacity", "This gym class has reached its maximum capacity.");

        public static readonly ValidationError ScheduledInThePast =
            new("GymClass.ScheduledInThePast", "Cannot schedule a class in the past.");

        public static readonly ValidationError InvalidClassType =
            new("GymClass.InvalidClassType", "The provided class type is invalid.");
    }

    public static class Booking
    {
        public static readonly NotFoundError NotFound =
            new("Booking.NotFound", "Booking not found.");

        public static readonly ValidationError PlanLimitReached =
            new("Booking.PlanLimitReached", "The student has reached the monthly booking limit for their plan.");

        public static readonly ConflictError AlreadyExists =
            new("Booking.AlreadyExists", "The student is already booked for this class.");

        public static readonly ValidationError ClassAlreadyStarted =
            new("Booking.ClassAlreadyStarted", "Cannot cancel a booking for a class that has already started.");
    }
}