// namespace portal.Mappings;

// using AutoMapper;
// using portal.DTOs;
// using portal.Models;

// public class GeneralWorkflowProfile : Profile
// {
//     public GeneralWorkflowProfile()
//     {
//         CreateMap<GeneralWorkflow, GeneralWorkflowDTO>()
//             .ForMember(dest => dest.GeneralInfo, opt => opt.MapFrom(src => src.GeneralWorkflowInfo))
//             .ForMember(
//                 dest => dest.ApprovalWorkflowNodesIds,
//                 opt => opt.MapFrom(src => src.ApprovalWorkflowNodes.Select(n => n.Id))
//             )
//             .ForMember(
//                 dest => dest.ApprovalWorkflowNodes,
//                 opt => opt.MapFrom(src => src.ApprovalWorkflowNodes)
//             );

//         CreateMap<GeneralWorkflowDTO, GeneralWorkflow>()
//             .ForMember(
//                 dest => dest.GeneralWorkflowInfo,
//                 opt => opt.MapFrom(src => src.GeneralInfo)
//             );

//         CreateMap<CreateGeneralWorkflowDTO, GeneralWorkflow>()
//             .ForMember(dest => dest.GeneralWorkflowInfo, opt => opt.MapFrom(src => src.GeneralInfo))
//             .ForMember(
//                 dest => dest.ApprovalWorkflowNodes,
//                 opt => opt.MapFrom(src => src.ApprovalWorkflowNodes)
//             );

//         CreateMap<UpdateGeneralWorkflowDTO, GeneralWorkflow>()
//             .ForMember(dest => dest.GeneralWorkflowInfo, opt => opt.MapFrom(src => src.GeneralInfo))
//             .ForMember(
//                 dest => dest.ApprovalWorkflowNodes,
//                 opt => opt.MapFrom(src => src.ApprovalWorkflowNodes)
//             );

//         CreateMap<GeneralWorkflowInfo, GeneralWorkflowInfoDTO>().ReverseMap();
//     }
// }
