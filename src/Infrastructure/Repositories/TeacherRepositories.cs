using Application.Interfaces.IRepositories;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class TeacherRepositories : ITeacherRepositories
{
    private readonly AppDbContext _context;

    public TeacherRepositories(AppDbContext context)
    {
        _context = context;
    }
    /*{ Get All Students data from DB }*/
    public async Task<IEnumerable<Teacher>> GetAllAsync() => 
        await _context.Teachers.ToListAsync();
    /*{ Get Student data from DB }*/
    public async Task<Teacher?> GetByIdAsync(int id) =>
        await _context.Teachers.FindAsync(id);
    /*{ Add Student data from DB }*/
    public async Task AddAsync(Teacher teacher)
    {
        await _context.Teachers.AddAsync(teacher);
        await _context.SaveChangesAsync();
    }
    /*{ Update Student data from DB }*/
    public async Task UpdateAsync(Teacher teacher)
    {
        var existteacher = await _context.Teachers.FindAsync(teacher.Id);
        if (existteacher != null)
        {
            existteacher.TeacherId = teacher.TeacherId;
            existteacher.FullName = teacher.FullName;
            existteacher.Email = teacher.Email;
            existteacher.Phone = teacher.Phone;
            existteacher.HireDate = teacher.HireDate;
            existteacher.Department = teacher.Department;
        }
        await _context.SaveChangesAsync();
    }
    /*{ Delete Student data from DB }*/
    public async Task RemoveAsync(int id)
    {
        var teachers = await _context.Teachers.FindAsync(id);
        if (teachers != null)
        {
            _context.Teachers.Remove(teachers);
            await _context.SaveChangesAsync();
        }
    }
}