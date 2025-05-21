namespace portal.Mappings;

using AutoMapper;
using portal.DTOs;
using portal.Models;

public class OrganizationEntityProfile : Profile
{
    public OrganizationEntityProfile()
    {
        CreateMap<CreateOrganizationEntityDTO, OrganizationEntity>();
        CreateMap<UpdateOrganizationEntityDTO, OrganizationEntity>()
            // Bỏ qua Id và MainID nếu không muốn cập nhật
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForAllMembers(opt =>
            {
                // Chỉ map khi source không null, và không phải Id
                opt.Condition(
                    (src, dest, srcMember, destMember) =>
                        srcMember != null && opt.DestinationMember.Name != nameof(BaseModel.Id)
                );
            });
        // Lên mapping sang DTO summary hoặc full:
        CreateMap<OrganizationEntity, OrganizationEntitySummaryDTO>()
            .ForMember(
                dest => dest.ParentName,
                opt => opt.MapFrom(src => src.Parent != null ? src.Parent.Name : null)
            )
            .ForMember(
                dest => dest.ChildrenIds,
                opt =>
                    opt.MapFrom(src =>
                        src.Children != null
                            ? src.Children.Select(c => c.Id).ToList()
                            : new List<int>()
                    )
            )
            .ForMember(
                dest => dest.ChildrenNames,
                opt =>
                    opt.MapFrom(src =>
                        src.Children != null
                            ? src.Children.Select(c => c.Name).ToList()
                            : new List<string>()
                    )
            )
            .ForMember(
                dest => dest.EmployeeNames,
                opt =>
                    opt.MapFrom(src =>
                        src.OrganizationEntityEmployees.Select(oe =>
                            oe.Employee.FirstName
                            + " "
                            + oe.Employee.MiddleName
                            + " "
                            + oe.Employee.LastName
                        )
                            .ToList()
                    )
            )
        // ... các mapping khác
        ;
    }
}
