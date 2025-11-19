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
            config.NewConfig<Course, CourseDto>();

            // CREATE DTO → ENTITY
            config.NewConfig<CourseCreateDto, Course>()
                .Ignore(d => d.Id);

            // UPDATE DTO → ENTITY
            config.NewConfig<CourseUpdateDto, Course>()
                .Ignore(d => d.Id);
        }
    }
}