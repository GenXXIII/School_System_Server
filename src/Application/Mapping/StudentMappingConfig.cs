using Application.DTOs.StudentDTO;
using Domain.Entities;
using Mapster;

namespace Application.Mapping
{
    public class StudentMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            // ENTITY → DTO (Get)
            config.NewConfig<Student, StudentDto>()
                .Map(dest => dest.DepartmentName, src => src.Department != null ? src.Department.DepartmentName : null);

            // CREATE DTO → ENTITY
            config.NewConfig<StudentCreateDto, Student>()
                .Ignore(d => d.Id);

            // UPDATE DTO → ENTITY
            config.NewConfig<StudentUpdateDto, Student>()
                .Ignore(d => d.Id);
        }
    }
}
