using AutoMapper;
using portal.DTOs;
using portal.Models;

namespace portal.Mappings;

public class ApprovalWorkflowNodeProfile : Profile
{
    public ApprovalWorkflowNodeProfile()
    {
        // Main entity <-> DTO
        CreateMap<ApprovalWorkflowNode, ApprovalWorkflowNodeDTO>()
            .ReverseMap()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());

        // Entity <-> Create DTO (ignore Id, timestamps on reverse)
        CreateMap<ApprovalWorkflowNode, CreateApprovalWorkflowNodeDTO>()
            .ReverseMap()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());

        // Entity <-> Update DTO (ignore Id, timestamps on reverse)
        CreateMap<ApprovalWorkflowNode, UpdateApprovalWorkflowNodeDTO>()
            .ReverseMap()
            .ForMember(dest => dest.Id, opt => opt.Ignore()) // Up to you; can allow if needed for patching
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());

        // Entity -> FileResult DTO
        CreateMap<ApprovalWorkflowNode, ApprovalWorkflowNodeFileResultDTO>()
            .ForMember(dest => dest.NodeId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.DocumentIds, opt => opt.Ignore())
            .ForMember(dest => dest.DocumentNames, opt => opt.Ignore())
            .ForMember(dest => dest.DocumentUrls, opt => opt.Ignore());
    }
}