using AutoMapper;
using portal.DTOs;
using portal.Models;

namespace portal.Mappings;

public class ApprovalWorkflowNodeProfile : Profile
{
    public ApprovalWorkflowNodeProfile()
    {
        CreateMap<ApprovalWorkflowNode, ApprovalWorkflowNodeDTO>();
        CreateMap<ApprovalWorkflowNodeDTO, ApprovalWorkflowNode>();

        CreateMap<ApprovalWorkflowNode, CreateApprovalWorkflowNodeDTO>()
            .ReverseMap()
            .ForMember(dest => dest.Id, opt => opt.Ignore()); // ignore Id during creation

        CreateMap<ApprovalWorkflowNode, UpdateApprovalWorkflowNodeDTO>().ReverseMap();

        CreateMap<ApprovalWorkflowNode, ApprovalWorkflowNodeFileResultDTO>()
            .ForMember(dest => dest.NodeId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.DocumentIds, opt => opt.Ignore())
            .ForMember(dest => dest.DocumentNames, opt => opt.Ignore())
            .ForMember(dest => dest.DocumentUrls, opt => opt.Ignore());
    }
}
