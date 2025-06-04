using AutoMapper;
using portal.DTOs;
using portal.Models;

namespace portal.Mappings;


public class LeaveRequestWorkflowProfile : Profile
{
    public LeaveRequestWorkflowProfile()
    {
        // ----------- GET MAPPING ----------- //
        CreateMap<LeaveRequestWorkflow, LeaveRequestWorkflowDTO>()
            .ForMember(dest => dest.EmployeeName, opt => opt.MapFrom(src => src.Employee.FirstName + " " + src.Employee.LastName))
            .ForMember(dest => dest.EmployeeAnnualLeaveTotalDays, opt => opt.MapFrom(src => src.Employee.CompanyInfo.AnnualLeaveTotalDays));

        CreateMap<LeaveRequestNode, LeaveRequestNodeDTO>()
            .ForMember(dest => dest.LeaveRequestName, opt => opt.MapFrom(src => src.LeaveRequestWorkflow.Name));

        // ----------- CREATE MAPPING ----------- //
        CreateMap<CreateLeaveRequestWorkflowDTO, LeaveRequestWorkflow>();

        CreateMap<CreateLeaveRequestNodeDTO, LeaveRequestNode>();

        // ----------- UPDATE MAPPING ----------- //
        CreateMap<UpdateLeaveRequestWorkflowDTO, LeaveRequestWorkflow>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

        CreateMap<UpdateLeaveRequestNodeDTO, LeaveRequestNode>()
             .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));
    }
}
