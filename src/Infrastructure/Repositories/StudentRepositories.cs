using Application.Interfaces.IRepositories;
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
    /*{ Get All Students data from DB }*/
    public async Task<IEnumerable<Student>> GetAllAsync() =>
        await _context.Students
            .Include(s => s.Department)
            .ToListAsync();
    /*{ Get Student data from DB }*/
    public async Task<Student?> GetByIdAsync(int id) =>
        await _context.Students
            .Include(s => s.Department)
            .FirstOrDefaultAsync(s => s.Id == id);
    /*{ Add Student data from DB }*/
    public async Task AddAsync(Student student) {
        
        await _context.Students.AddAsync(student);
        await _context.SaveChangesAsync();
    }
    /*{ Update Student data from DB }*/
    public async Task UpdateAsync(Student student)
    {
        var existstudent = await _context.Students.FindAsync(student.Id);
        if (existstudent != null)
        {
            existstudent.StudentId = student.StudentId;
            existstudent.FullName = student.FullName;
            existstudent.Gender = student.Gender;
            existstudent.BirthDate = student.BirthDate;
            existstudent.Address = student.Address;
            existstudent.Email = student.Email;
            existstudent.Phone = student.Phone;
            existstudent.Address = student.Address;
            existstudent.Year = student.Year;
            existstudent.DepartmentId = student.DepartmentId;
        }
        await _context.SaveChangesAsync();
    }
    /*{ Remove Student data from DB }*/
    public async Task RemoveAsync(int id)
    {
        var student = await _context.Students.FindAsync(id);
        if (student != null)
        {
            _context.Students.Remove(student);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> IsStudentIdUniqueAsync(string studentId)
    {
        return !await _context.Students.AnyAsync(s => s.StudentId == studentId);
    }
}