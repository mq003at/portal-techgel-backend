using AutoMapper;
using portal.DTOs;
using portal.Models;

public class TeamProfile : Profile
{
    public TeamProfile()
    {
        CreateMap<Team, TeamDTO>().ReverseMap();

        CreateMap<UpdateTeamDTO, Team>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
    }
}
