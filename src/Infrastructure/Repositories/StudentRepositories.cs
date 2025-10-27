using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class StudentRepositories : IStudentRepositories
{
    private readonly AppDbContext _context;

    public StudentRepositories(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Student>> GetAllAsync() =>
        await _context.Students.ToListAsync();

    public async Task<Student?> GetByIdAsync(int id) =>
        await _context.Students.FindAsync(id);

    public async Task AddAsync(Student student) {
        
        await _context.Students.AddAsync(student);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(int id,Student student)
    {
        var students = await _context.Students.FindAsync(id);
        if (students != null)
        {
            _context.Students.Update(student);
            await _context.SaveChangesAsync();
        }
    }

    public async Task RemoveAsync(int id)
    {
        var student = await _context.Students.FindAsync(id);
        if (student != null)
        {
            _context.Students.Remove(student);
            await _context.SaveChangesAsync();
        }
    }
}