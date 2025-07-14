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
            .ForMember(dest => dest.RaciRole, opt => opt.MapFrom(src => src.RaciRole))
            .ForMember(
                dest => dest.EmployeeName,
                opt =>
                    opt.MapFrom(src =>
                        src.Employee.LastName
                        + " "
                        + src.Employee.MiddleName
                        + " "
                        + src.Employee.FirstName
                    )
            )
            .ForMember(dest => dest.WorkflowNodeName, opt => opt.Ignore()) // No navigation, must be added manually
            .ForMember(dest => dest.ApprovalStatus, opt => opt.MapFrom(src => src.ApprovalStatus));

        // Create DTO ➜ Entity
        CreateMap<WorkflowNodeParticipantCreateDTO, WorkflowNodeParticipant>();

        // Update DTO ➜ Entity
        CreateMap<WorkflowNodeParticipantUpdateDTO, WorkflowNodeParticipant>();
    }
}
