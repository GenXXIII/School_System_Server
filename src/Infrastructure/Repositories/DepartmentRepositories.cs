using Application.Interfaces.IRepositories;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class DepartmentRepositories : IDepartmentRepositories
{
    private readonly AppDbContext _context;

    public DepartmentRepositories(AppDbContext context)
    {
        _context = context;
    }
    /*{Get All Department data from DB}*/
    public async Task<IEnumerable<Department>> GetAllAsync() => 
        await _context.Departments
            .Include(c => c.Students)
            .ToListAsync();
    /*{Get Department data from DB}*/
    public async Task<Department?> GetByIdAsync(int id) => 
        await _context.Departments
            .Include(c => c.Students)
            .FirstOrDefaultAsync(c => c.Id == id);
    /*{Add Department data from DB}*/
    public async Task AddAsync(Department department)
    {
        await _context.Departments.AddAsync(department);
        await _context.SaveChangesAsync();
    }
    /*{Update Department data from DB}*/
    public async Task UpdateAsync(Department department)
    {
        var existDepartment = await _context.Departments.FindAsync(department.Id);
        if (existDepartment != null)
        {
            existDepartment.DepartmentId = department.DepartmentId;
            existDepartment.DepartmentName = department.DepartmentName;
            existDepartment.RoomNumber = department.RoomNumber;
            existDepartment.Building = department.Building;
            await _context.SaveChangesAsync();
        }
    }
    /*{Delete Department data from DB}*/
    public async Task RemoveAsync(int id)
    {
        var department = await _context.Departments.FindAsync(id);
        if (department != null)
        {
            _context.Departments.Remove(department);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> IsDepartmentNameUniqueAsync(string departmentName)
    {
        return !await _context.Departments.AnyAsync(c => c.DepartmentName == departmentName);
    }
}
