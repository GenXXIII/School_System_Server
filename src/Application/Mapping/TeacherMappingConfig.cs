using Application.DTOs.TeacherDTO;
using Domain.Entities;
using Mapster;

namespace Application.Mapping
{
    public class TeacherMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            // ENTITY → DTO (Get)
            config.NewConfig<Teacher, TeacherDto>()
                .Map(dest => dest.CourseName, src => src.Course != null ? src.Course.CourseName : null);
                //.Map(dest => dest.DepartmentName, src => src.Department != null ? src.Department.DepartmentName : null)
                //.Map(dest => dest.DepartmentId, src => src.DepartmentId)
                //.Map(dest => dest.Courses, src => src.Courses.Select(c => c.CourseName).ToList());

            // CREATE DTO → ENTITY
            config.NewConfig<TeacherCreateDto, Teacher>()
                .Ignore(d => d.Id);

            // UPDATE DTO → ENTITY
            config.NewConfig<TeacherUpdateDto, Teacher>()
                .Ignore(d => d.Id);
        }
    }
}
