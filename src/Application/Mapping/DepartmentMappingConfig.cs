using Application.DTOs.DepartmentDTO;
using Domain.Entities;
using Mapster;

namespace Application.Mapping
{
    public class DepartmentMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Department, DepartmentDto>();

            config.NewConfig<DepartmentCreateDto, Department>()
                .Ignore(d => d.Id);

            config.NewConfig<DepartmentUpdateDto, Department>()
                .Ignore(d => d.Id);
        }
    }
}
