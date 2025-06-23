using AutoMapper;
using portal.DTOs;
using portal.Models;

namespace portal.Mappings;
public class ScheduleInfoProfile : Profile
{
    public ScheduleInfoProfile()
    {
        // --------- Entity ↔ Read DTO --------- //
        CreateMap<ScheduleInfo, ScheduleInfoDTO>()
            .ForMember(dest => dest.TotalWeeklyHours, opt => opt.MapFrom(src => src.TotalWeeklyHours))
            .ReverseMap();

        // --------- Create DTO → Entity --------- //
        CreateMap<CreateScheduleInfoDTO, ScheduleInfo>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Employee, opt => opt.Ignore());

        // --------- Update DTO → Entity --------- //
        CreateMap<UpdateScheduleInfoDTO, ScheduleInfo>()
            .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));
    }
}