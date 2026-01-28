using Application.DTOs.CourseDTO;
using Domain.Entities;
using Mapster;

namespace Application.Mapping
{
    public class CourseMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            // ENTITY → DTO (Get)
            config.NewConfig<Course, CourseDto>()
                //.Map(dest => dest.TeacherName, src => src.Teacher != null ? src.Teacher.FullName : null)
                .Map(dest => dest.DepartmentName, src => src.Department != null ? src.Department.DepartmentName : null);

            // CREATE DTO → ENTITY
            config.NewConfig<CourseCreateDto, Course>()
                .Ignore(d => d.Id);

            // UPDATE DTO → ENTITY
            config.NewConfig<CourseUpdateDto, Course>()
                .Ignore(d => d.Id);
        }
    }
}
