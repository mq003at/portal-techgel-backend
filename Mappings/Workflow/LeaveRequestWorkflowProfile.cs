using AutoMapper;
using portal.DTOs;
using portal.Models;

namespace portal.Mappings;


public class LeaveRequestWorkflowProfile : BaseWorkflowProfile<
    LeaveRequestWorkflow,
    LeaveRequestWorkflowDTO,
    LeaveRequestWorkflowCreateDTO,
    LeaveRequestWorkflowUpdateDTO,
    BaseModel,
    BaseModelDTO,
    BaseModelCreateDTO,
    BaseModelUpdateDTO>
{
    public LeaveRequestWorkflowProfile()
        : base()
    {
        CreateMap<LeaveRequestWorkflow, LeaveRequestWorkflowDTO>()
            .IncludeBase<BaseWorkflow, BaseWorkflowDTO>()
            .ForMember(dest => dest.LeaveRequestNodes, opt => opt.MapFrom(src => src.LeaveRequestNodes))
            .ReverseMap();

    }
}