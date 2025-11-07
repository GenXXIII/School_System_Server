using Application.Interfaces.IRepositories;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class GradeRepositories : IGradeRepositories
{
    private readonly AppDbContext _context;

    public GradeRepositories(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Grade>> GetAllAsync() => await _context.Grades.ToListAsync();
    public async Task<Grade?> GetByIdAsync(int id) => await _context.Grades.FindAsync(id);
    public async Task AddAsync(Grade grade) {
        await _context.Grades.AddAsync(grade);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Grade grade)
    {
        var existgrade = await _context.Grades.FindAsync(grade.Id);
        if (existgrade != null)
        {
            existgrade.GradeId = grade.GradeId;
            existgrade.Score = grade.Score;
            await _context.SaveChangesAsync();
        }
    }

    public async Task RemoveAsync(int id)
    {
        var grades = await _context.Grades.FindAsync(id);
        if (grades != null)
        {
            _context.Grades.Remove(grades);
            await _context.SaveChangesAsync();
        }
    }
}