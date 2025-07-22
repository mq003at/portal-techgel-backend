using AutoMapper;
using portal.DTOs;
using portal.Enums;
using portal.Extensions;
using portal.Models;

namespace portal.Mappings;

public class GatePassWorkflowProfile
    : BaseWorkflowProfile<
        GatePassWorkflow,
        GatePassWorkflowDTO,
        GatePassWorkflowCreateDTO,
        GatePassWorkflowUpdateDTO,
        BaseModel,
        BaseModelDTO,
        BaseModelCreateDTO,
        BaseModelUpdateDTO
    >
{
    public GatePassWorkflowProfile()
    {
        // Mappings specific to GatePassWorkflow

        // GatePassWorkflow <-> GatePassWorkflowDTO
        CreateMap<GatePassWorkflow, GatePassWorkflowDTO>()
            .IncludeBase<BaseWorkflow, BaseWorkflowDTO>()
            .ForMember(dest => dest.Reason, opt => opt.MapFrom(src => src.Reason))
            .ForMember(dest => dest.GatePassNodes, opt => opt.MapFrom(src => src.GatePassNodes))
            .ForMember(dest => dest.Comment, opt => opt.MapFrom(src => src.Comment))
            .ForMember(dest => dest.SenderMainId, opt => opt.MapFrom(src => src.Sender.MainId))
            .ForMember(dest => dest.SenderName, opt => opt.MapFrom(src => src.Sender.LastName + " " + src.Sender.MiddleName + " " + src.Sender.FirstName))
            .ReverseMap();

        // GatePassWorkflowCreateDTO <-> GatePassWorkflow (for creating workflows)
        CreateMap<GatePassWorkflowCreateDTO, GatePassWorkflow>()
            .IncludeBase<BaseModelCreateDTO, BaseModel>()
            .ForMember(dest => dest.Comment, opt => opt.MapFrom(src => ""))
            .ForMember(dest => dest.Reason, opt => opt.MapFrom(src => src.Reason))
            .ForMember(
                dest => dest.GatePassStartTime,
                opt => opt.MapFrom(src => src.GatePassStartTime)
            )
            .ForMember(
                dest => dest.GatePassEndTime,
                opt => opt.MapFrom(src => src.GatePassEndTime)
            );

        // GatePassWorkflowUpdateDTO <-> GatePassWorkflow (for updating workflows)
        CreateMap<GatePassWorkflowUpdateDTO, GatePassWorkflow>()
            .IncludeBase<BaseModelUpdateDTO, BaseModel>()
            .ForAllMembers(opt =>
            {
                opt.Condition((src, dest, srcMember) => srcMember != null);
            });
        // Reverse mapping for update is typically handled here as well
    }
}
