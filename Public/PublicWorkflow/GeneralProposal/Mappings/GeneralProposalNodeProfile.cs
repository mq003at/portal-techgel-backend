namespace portal.Mappings;

using portal.DTOs;
using portal.Models;

public class GeneralProposalNodeProfile
    : BaseWorkflowNodeProfile<
        LeaveRequestNode,
        LeaveRequestNodeDTO,
        LeaveRequestNodeCreateDTO,
        LeaveRequestNodeUpdateDTO,
        BaseModel,
        BaseModelDTO,
        BaseModelCreateDTO,
        BaseModelUpdateDTO
    >
{
    public GeneralProposalNodeProfile()
        : base()
    {
        /* ------------ base maps that were missing earlier ------------ */
        CreateMap<BaseWorkflowNode, WorkflowNodeDTO>()
            .ReverseMap();
        CreateMap<WorkflowNodeCreateDTO, BaseWorkflowNode>();
        CreateMap<WorkflowNodeUpdateDTO, BaseWorkflowNode>();

        /* ------------ entity ➜ DTO ------------ */
        CreateMap<GeneralProposalNode, GeneralProposalNodeDTO>()
            .IncludeBase<BaseWorkflowNode, WorkflowNodeDTO>()
            .ForMember(d => d.StepType, o => o.MapFrom(s => s.StepType))
            .ForMember(d => d.StepTypeName, o => o.Ignore())
            .AfterMap(
                (src, dest) =>
                {
                    dest.StepTypeName = src.StepType.ToString();
                }
            );

        /* ------------ DTO ➜ entity (create / update) ------------ */
        CreateMap<GeneralProposalNodeCreateDTO, GeneralProposalNode>()
            .IncludeBase<WorkflowNodeCreateDTO, BaseWorkflowNode>()
            .ForMember(d => d.StepType, o => o.MapFrom(s => s.StepType));

        CreateMap<GeneralProposalNodeUpdateDTO, GeneralProposalNode>()
            .IncludeBase<WorkflowNodeUpdateDTO, BaseWorkflowNode>()
            .ForMember(d => d.StepType, o => o.MapFrom(s => s.StepType));

        // Optional: if you ever need DTO → entity straight from GeneralProposalNodeDTO
        CreateMap<GeneralProposalNodeDTO, GeneralProposalNode>()
            .IncludeBase<WorkflowNodeDTO, BaseWorkflowNode>()
            .ForMember(d => d.StepType, o => o.MapFrom(s => s.StepType))
            .ForMember(d => d.Workflow, o => o.Ignore());
    }
}
