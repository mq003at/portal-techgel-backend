using AutoMapper;
using portal.DTOs;
using portal.Enums;
using portal.Extensions;
using portal.Models;

namespace portal.Mappings;

public class GeneralProposalWorkflowProfile
    : BaseWorkflowProfile<
        GeneralProposalWorkflow,
        GeneralProposalWorkflowDTO,
        GeneralProposalWorkflowCreateDTO,
        GeneralProposalWorkflowUpdateDTO,
        BaseModel,
        BaseModelDTO,
        BaseModelCreateDTO,
        BaseModelUpdateDTO
    >
{
    public GeneralProposalWorkflowProfile()
    {
        // Mappings specific to GeneralProposalWorkflow

        // GeneralProposalWorkflow <-> GeneralProposalWorkflowDTO
        CreateMap<GeneralProposalWorkflow, GeneralProposalWorkflowDTO>()
            .IncludeBase<BaseWorkflow, BaseWorkflowDTO>()
            .ForMember(dest => dest.ApproverMainId, opt => opt.MapFrom(src => src.Approver.MainId))
            .ForMember(dest => dest.ApproverId, opt => opt.MapFrom(src => src.Approver.Id))
            .ForMember(
                dest => dest.ApproverName,
                opt => opt.MapFrom(src => src.Approver.GetDisplayName())
            )
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
            .ForMember(dest => dest.ProjectName, opt => opt.MapFrom(src => src.ProjectName))
            .ForMember(dest => dest.Comment, opt => opt.MapFrom(src => src.Comment))
            .ForMember(dest => dest.Reason, opt => opt.MapFrom(src => src.Reason))
            .ForMember(dest => dest.Proposal, opt => opt.MapFrom(src => src.Proposal))
            .ForMember(dest => dest.SenderMainId, opt => opt.MapFrom(src => src.Sender.MainId))
            .ForMember(dest => dest.SenderName, opt => opt.MapFrom(src => src.Sender.GetDisplayName()))
            .ForMember(
                dest => dest.GeneralProposalNodes,
                opt => opt.MapFrom(src => src.GeneralProposalNodes)
            )
            .ReverseMap();

        // GeneralProposalWorkflowCreateDTO <-> GeneralProposalWorkflow (for creating workflows)
        CreateMap<GeneralProposalWorkflowCreateDTO, GeneralProposalWorkflow>()
            .IncludeBase<BaseModelCreateDTO, BaseModel>()
            .ForMember(dest => dest.About, opt => opt.MapFrom(src => src.About))
            .ForMember(dest => dest.Comment, opt => opt.MapFrom(src => ""))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
            .ForMember(dest => dest.Reason, opt => opt.MapFrom(src => src.Reason))
            .ForMember(dest => dest.Proposal, opt => opt.MapFrom(src => src.Proposal))
            .ForMember(dest => dest.ProjectName, opt => opt.MapFrom(src => src.ProjectName)) // Map the project field appropriately
            .ForMember(dest => dest.ApproverId, opt => opt.MapFrom(src => src.ApproverId)); // ApproverId comes from EmployeeId in CreateDTO

        // GeneralProposalWorkflowUpdateDTO <-> GeneralProposalWorkflow (for updating workflows)
        CreateMap<GeneralProposalWorkflowUpdateDTO, GeneralProposalWorkflow>()
            .IncludeBase<BaseModelUpdateDTO, BaseModel>()
            .ForAllMembers(opt =>
            {
                opt.Condition((src, dest, srcMember) => srcMember != null);
            });
        // Reverse mapping for update is typically handled here as well
    }
}
