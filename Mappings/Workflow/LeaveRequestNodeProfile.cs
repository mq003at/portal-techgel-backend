namespace portal.Mappings;

using portal.DTOs;
using portal.Models;

public class LeaveRequestNodeProfile : BaseWorkflowNodeProfile<
    LeaveRequestNode,
    LeaveRequestNodeDTO,
    LeaveRequestNodeCreateDTO,
    LeaveRequestNodeUpdateDTO,
    BaseModel,
    BaseModelDTO,
    BaseModelCreateDTO,
    BaseModelUpdateDTO>
{
    public LeaveRequestNodeProfile()
        : base()
    {
        CreateMap<LeaveRequestNode, LeaveRequestNodeDTO>()
            .IncludeBase<BaseWorkflowNode, WorkflowNodeDTO>()
            .ForMember(dest => dest.StepType, opt => opt.MapFrom(src => src.StepType))
            .ForMember(dest => dest.StepTypeName, opt => opt.MapFrom(src => src.StepType.ToString()));

        CreateMap<LeaveRequestNodeCreateDTO, LeaveRequestNode>()
            .IncludeBase<WorkflowNodeCreateDTO, BaseWorkflowNode>()
            .ForMember(dest => dest.StepType, opt => opt.MapFrom(src => src.StepType));

        CreateMap<LeaveRequestNodeUpdateDTO, LeaveRequestNode>()
            .IncludeBase<WorkflowNodeUpdateDTO, BaseWorkflowNode>()
            .ForMember(dest => dest.StepType, opt => opt.MapFrom(src => src.StepType));
    }
}
