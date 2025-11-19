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
            config.NewConfig<Teacher, TeacherDto>();

            // CREATE DTO → ENTITY
            config.NewConfig<TeacherCreateDto, Teacher>()
                .Ignore(d => d.Id);

            // UPDATE DTO → ENTITY
            config.NewConfig<TeacherUpdateDto, Teacher>()
                .Ignore(d => d.Id);
        }
    }
}