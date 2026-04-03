using GymClassBooking.Application.Interfaces;
using GymClassBooking.Application.UseCases.Bookings;
using GymClassBooking.Application.UseCases.GymClasses;
using GymClassBooking.Application.UseCases.Reports;
using GymClassBooking.Application.UseCases.Students;
using GymClassBooking.Infrastructure.Persistence;
using GymClassBooking.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GymClassBooking.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlite(configuration.GetConnectionString("DefaultConnection")));

        // Repositories
        services.AddScoped<IStudentRepository, StudentRepository>();
        services.AddScoped<IGymClassRepository, GymClassRepository>();
        services.AddScoped<IBookingRepository, BookingRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // Use Cases
        services.AddScoped<CreateStudent>();
        services.AddScoped<GetAllStudents>();
        services.AddScoped<CreateGymClass>();
        services.AddScoped<GetAllGymClasses>();
        services.AddScoped<CreateBooking>();
        services.AddScoped<CancelBooking>();
        services.AddScoped<GetAllBookings>();
        services.AddScoped<GetStudentReport>();

        return services;
    }
}