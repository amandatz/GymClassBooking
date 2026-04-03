using GymClassBooking.Application.Interfaces;
using GymClassBooking.Domain.Entities;
using GymClassBooking.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GymClassBooking.Infrastructure.Repositories;

public sealed class StudentRepository(AppDbContext context) : IStudentRepository
{
    public async Task<Student?> GetByIdAsync(Guid id, CancellationToken ct = default) =>
        await context.Students.FindAsync([id], ct);

    public async Task<Student?> GetByEmailAsync(string email, CancellationToken ct = default) =>
        await context.Students.FirstOrDefaultAsync(s => s.Email == email, ct);

    public async Task<IReadOnlyList<Student>> GetAllAsync(CancellationToken ct = default) =>
        await context.Students.ToListAsync(ct);

    public async Task AddAsync(Student student, CancellationToken ct = default) =>
        await context.Students.AddAsync(student, ct);
}