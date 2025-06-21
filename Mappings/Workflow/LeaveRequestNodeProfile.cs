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
        /* ------------ base maps that were missing earlier ------------ */
        CreateMap<BaseWorkflowNode, WorkflowNodeDTO>().ReverseMap();
        CreateMap<WorkflowNodeCreateDTO, BaseWorkflowNode>();
        CreateMap<WorkflowNodeUpdateDTO, BaseWorkflowNode>();

        /* ------------ entity ➜ DTO ------------ */
        CreateMap<LeaveRequestNode, LeaveRequestNodeDTO>()
            .IncludeBase<BaseWorkflowNode, WorkflowNodeDTO>()

            // Let EF translate this; names match, so no config needed:
            .ForMember(d => d.StepType, o => o.MapFrom(s => s.StepType))

            // 👇 Disable server-side translation …
            .ForMember(d => d.StepTypeName, o => o.Ignore())

            // 👇 … and fill it after the list is in memory
            .AfterMap((src, dest) =>
            {
                dest.StepTypeName = src.StepType.ToString();
            });

        /* ------------ DTO ➜ entity (create / update) ------------ */
        CreateMap<LeaveRequestNodeCreateDTO, LeaveRequestNode>()
            .IncludeBase<WorkflowNodeCreateDTO, BaseWorkflowNode>()
            .ForMember(d => d.StepType, o => o.MapFrom(s => s.StepType));

        CreateMap<LeaveRequestNodeUpdateDTO, LeaveRequestNode>()
            .IncludeBase<WorkflowNodeUpdateDTO, BaseWorkflowNode>()
            .ForMember(d => d.StepType, o => o.MapFrom(s => s.StepType));

        // Optional: if you ever need DTO → entity straight from LeaveRequestNodeDTO
        CreateMap<LeaveRequestNodeDTO, LeaveRequestNode>()
            .IncludeBase<WorkflowNodeDTO, BaseWorkflowNode>()
            .ForMember(d => d.StepType, o => o.MapFrom(s => s.StepType))
            .ForMember(d => d.Workflow, o => o.Ignore()); // avoid circular refs
    }
}
