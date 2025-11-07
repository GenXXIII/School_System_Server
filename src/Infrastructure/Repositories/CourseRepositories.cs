using Application.Interfaces.IRepositories;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class CourseRepositories : ICourseRepositories
{
    private readonly AppDbContext _context;

    public CourseRepositories(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Course>> GetAllAsync() => await _context.Courses.ToListAsync();
    public async Task<Course?> GetByIdAsync(int id) => await _context.Courses.FindAsync(id);

    public async Task AddAsync(Course course)
    {
        await _context.Courses.AddAsync(course);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Course course)
    {
        var existcourse = await _context.Courses.FindAsync(course.Id);
        if (existcourse != null)
        {
            existcourse.CourseId = course.CourseId;
            existcourse.CourseName = course.CourseName;
            existcourse.Desc = course.Desc;
            await _context.SaveChangesAsync();
        }

    }

    public async Task RemoveAsync(int id)
    {
        var courses = await _context.Courses.FindAsync(id);
        if (courses != null)
        {
            _context.Courses.Remove(courses);
            await _context.SaveChangesAsync();
        }
    }
}