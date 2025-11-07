using Application.Interfaces.IRepositories;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class ClassroomRepositories : IClassroomRepositories
{
    private readonly AppDbContext _context;

    public ClassroomRepositories(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Classroom>> GetAllAsync() => await _context.Classrooms.ToListAsync();
    public async Task<Classroom?> GetByIdAsync(int id) => await _context.Classrooms.FindAsync(id);
    public async Task AddAsync(Classroom classroom)
    {
        await _context.Classrooms.AddAsync(classroom);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Classroom classroom)
    {
        var existclassroom = await _context.Classrooms.FindAsync(classroom.Id);
        if (existclassroom != null)
        {
            existclassroom.ClassId = classroom.ClassId;
            existclassroom.Classname =  classroom.Classname;
            existclassroom.RoomNumber = classroom.RoomNumber;
            existclassroom.Building = classroom.Building;
            await _context.SaveChangesAsync();
        }
    }

    public async Task RemoveAsync(int id)
    {
        var  classroom = await _context.Classrooms.FindAsync(id);
        if (classroom != null)
        {
            _context.Classrooms.Remove(classroom);
            await _context.SaveChangesAsync();
        }
    }
}