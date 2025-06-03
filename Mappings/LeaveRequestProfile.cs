using AutoMapper;
using portal.DTOs;
using portal.Models;

namespace portal.Mappings;


public class LeaveRequestProfile : Profile
{
    public LeaveRequestProfile()
    {
        // ----------- GET MAPPING ----------- //
        CreateMap<LeaveRequestWorkflow, LeaveRequestWorkflowDTO>()
            .ForMember(dest => dest.EmployeeName, opt => opt.MapFrom(src => src.Employee.FirstName + " " + src.Employee.LastName))
            .ForMember(dest => dest.EmployeeAnnualLeaveTotalDays, opt => opt.MapFrom(src => src.Employee.CompanyInfo.AnnualLeaveTotalDays));

        CreateMap<LeaveRequestNode, LeaveRequestNodeDTO>()
            .ForMember(dest => dest.LeaveRequestName, opt => opt.MapFrom(src => src.LeaveRequestWorkflow.Name));

        // ----------- CREATE MAPPING ----------- //
        CreateMap<CreateLeaveRequestWorkflowDTO, LeaveRequestWorkflow>()
            .ForMember(dest => dest.LeaveRequestNodes, opt => opt.MapFrom(src => src.LeaveRequestNodes));

        CreateMap<CreateLeaveRequestNodeDTO, LeaveRequestNode>();

        // ----------- UPDATE MAPPING ----------- //
        CreateMap<UpdateLeaveRequestWorkflowDTO, LeaveRequestWorkflow>()
            .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

        CreateMap<UpdateLeaveRequestNodeDTO, LeaveRequestNode>()
            .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));
    }
}
