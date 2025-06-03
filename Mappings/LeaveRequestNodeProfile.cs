namespace portal.Mappings;

using AutoMapper;
using portal.DTOs;
using portal.Models;

public class LeaveRequestNodeProfile : Profile
{
    public LeaveRequestNodeProfile()
    {
        // ----------- GET MAPPING ----------- //
        CreateMap<LeaveRequestNode, LeaveRequestNodeDTO>()
            .ForMember(dest => dest.LeaveRequestName, opt => opt.MapFrom(src => src.LeaveRequestWorkflow.Name));

        // ----------- CREATE MAPPING ----------- //
        CreateMap<CreateLeaveRequestNodeDTO, LeaveRequestNode>();

        // ----------- UPDATE MAPPING ----------- //
        CreateMap<UpdateLeaveRequestNodeDTO, LeaveRequestNode>()
            .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));
    }
}