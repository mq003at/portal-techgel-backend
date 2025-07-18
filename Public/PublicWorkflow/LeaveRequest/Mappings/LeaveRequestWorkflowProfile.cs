using AutoMapper;
using portal.DTOs;
using portal.Enums;
using portal.Extensions;
using portal.Models;

namespace portal.Mappings;

public class LeaveRequestWorkflowProfile
    : BaseWorkflowProfile<
        LeaveRequestWorkflow,
        LeaveRequestWorkflowDTO,
        LeaveRequestWorkflowCreateDTO,
        LeaveRequestWorkflowUpdateDTO,
        BaseModel,
        BaseModelDTO,
        BaseModelCreateDTO,
        BaseModelUpdateDTO
    >
{
    public LeaveRequestWorkflowProfile()
        : base()
    {
        CreateMap<LeaveRequestWorkflow, LeaveRequestWorkflowDTO>()
            .IncludeBase<BaseWorkflow, BaseWorkflowDTO>()
            .ForMember(
                dest => dest.EmployeeMainId,
                opt => opt.MapFrom(src => src.Employee != null ? src.Employee.MainId : string.Empty)
            )
            .ForMember(
                dest => dest.EmployeeName,
                opt => opt.MapFrom(src => src.Employee.GetDisplayName())
            )
            .ForMember(
                dest => dest.LeaveRequestNodes,
                opt => opt.MapFrom(src => src.LeaveRequestNodes)
            )
            .ForMember(dest => dest.Comment, opt => opt.MapFrom(src => src.Comment))
            .ForMember(dest => dest.AssigneeDetails, opt => opt.MapFrom(src => src.AssigneeDetails))
            .ForMember(dest => dest.AssigneeNames, opt => opt.MapFrom(src => src.AssigneeNames))
            .ReverseMap();

        CreateMap<LeaveRequestWorkflowCreateDTO, LeaveRequestWorkflow>()
            .IncludeBase<BaseModelCreateDTO, BaseModel>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => "Nghỉ phép nhân viên"))
            .ForMember(dest => dest.Reason, opt => opt.MapFrom(src => src.Reason))
            .ForMember(dest => dest.AssigneeDetails, opt => opt.MapFrom(src => src.AssigneeDetails))
            .ForMember(
                dest => dest.Status,
                opt => opt.MapFrom(src => GeneralWorkflowStatusType.DRAFT)
            )
            .ForMember(
                dest => dest.MainId,
                opt =>
                    opt.MapFrom(src =>
                        "ĐN-"
                        + "TG"
                        + src.EmployeeId.ToString().PadLeft(5, '0')
                        + "-"
                        + src.StartDate.ToString("dd.MM")
                        + "-"
                        + src.EndDate.ToString("dd.MM")
                    )
            )
            // .ForMember(dest => dest.Description, opt => opt.MapFrom((src, dest) =>
            //     "Hồ sơ nghỉ phép nhân viên " + src.EmployeeId +
            //     " Từ: " + src.StartDate.ToString("HH:mm dd/MM/yyyy") +
            //     " tới ngày " + src.EndDate.ToString("HH:mm dd/MM/yyyy")))
            .ForMember(dest => dest.LeaveRequestNodes, opt => opt.Ignore());
    }
}
