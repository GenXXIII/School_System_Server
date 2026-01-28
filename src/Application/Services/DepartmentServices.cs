using Application.DTOs.DepartmentDTO;
using Application.Interfaces.IRepositories;
using Application.Interfaces.IServices;
using Domain.Entities;
using Mapster;
using FluentValidation;

namespace Application.Services;
/*{ Communication between DB and Backend by using DTO }*/
public class DepartmentServices : IDepartmentServices
{
    private readonly IDepartmentRepositories _repo;
    public DepartmentServices(IDepartmentRepositories repo)
    {
        _repo = repo;
    }
    public async Task<IEnumerable<DepartmentDto>> GetAllAsync()
    {
        var departments = await _repo.GetAllAsync();
        return departments.Adapt<IEnumerable<DepartmentDto>>();
    }

    public async Task<DepartmentDto?> GetByIdAsync(int id)
    {
        var department = await _repo.GetByIdAsync(id);
        return department?.Adapt<DepartmentDto>();
    }

    public async Task AddAsync(DepartmentCreateDto dto)
    {
        if (dto.DepartmentName != null && !await _repo.IsDepartmentNameUniqueAsync(dto.DepartmentName))
            throw new ValidationException("Department Name already exists.");

        var department = dto.Adapt<Department>();
        await _repo.AddAsync(department);
    }

    public async Task UpdateAsync(int id, DepartmentUpdateDto dto)
    {
        var existing = await _repo.GetByIdAsync(id);
        if (existing == null)
            throw new KeyNotFoundException($"Department with ID {id} not found.");

        if (!string.IsNullOrEmpty(dto.DepartmentName) && dto.DepartmentName != existing.DepartmentName)
        {
            if (!await _repo.IsDepartmentNameUniqueAsync(dto.DepartmentName!))
                throw new ValidationException("Department Name already exists.");
        }

        var department = dto.Adapt<Department>();
        department.Id = id;
        await _repo.UpdateAsync(department);
    }

    public async Task RemoveAsync(int id)
    {
        var existing = await _repo.GetByIdAsync(id);
        if (existing == null)
            throw new KeyNotFoundException($"Department with ID {id} not found.");

        await _repo.RemoveAsync(id);
    }
}
