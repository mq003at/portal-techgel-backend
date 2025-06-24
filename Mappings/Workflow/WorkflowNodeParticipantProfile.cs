namespace portal.Mappings;

using AutoMapper;
using portal.DTOs;
using portal.Models;

public class WorkflowNodeParticipantProfile : Profile
{
    public WorkflowNodeParticipantProfile()
    {
        // Entity ➜ Read DTO
        CreateMap<WorkflowNodeParticipant, WorkflowNodeParticipantDTO>()
            .ForMember(dest => dest.EmployeeName,
                opt => opt.MapFrom(src =>
                    src.Employee.FirstName + " " + src.Employee.LastName))
            .ForMember(dest => dest.WorkflowNodeName,
                opt => opt.Ignore()) // No navigation, must be added manually
            .ForMember(dest => dest.NodeStep,
                opt => opt.MapFrom(src => src.WorkflowNodeStepType))
            .ForMember(dest => dest.IsApproved,
                opt => opt.MapFrom(src => src.HasApproved))
            .ForMember(dest => dest.IsRejected,
                opt => opt.MapFrom(src => src.HasRejected));

        // Create DTO ➜ Entity
        CreateMap<WorkflowNodeParticipantCreateDTO, WorkflowNodeParticipant>()
            .ForMember(dest => dest.WorkflowNodeStepType,
                opt => opt.MapFrom(src => src.NodeStep))
            .ForMember(dest => dest.HasApproved, opt => opt.Ignore())
            .ForMember(dest => dest.HasRejected, opt => opt.Ignore())
            .ForMember(dest => dest.ApprovalDate, opt => opt.Ignore())
            .ForMember(dest => dest.ApprovalStartDate, opt => opt.Ignore())
            .ForMember(dest => dest.TAT, opt => opt.Ignore());

        // Update DTO ➜ Entity
        CreateMap<WorkflowNodeParticipantUpdateDTO, WorkflowNodeParticipant>()
            .ForMember(dest => dest.WorkflowNodeStepType,
                opt => opt.MapFrom(src => src.NodeStep))
            .ForMember(dest => dest.HasApproved,
                opt => opt.MapFrom(src => src.IsApproved))
            .ForMember(dest => dest.HasRejected,
                opt => opt.MapFrom(src => src.IsRejected));
    }
}