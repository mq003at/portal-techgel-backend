namespace portal.Services;

using AutoMapper;
using portal.Db;
using portal.DTOs;
using portal.Models;

public class TeamService : BaseService<Team, TeamDTO, UpdateTeamDTO>, ITeamService
{
    public TeamService(ApplicationDbContext context, IMapper mapper, ILogger<TeamService> logger)
        : base(context, mapper, logger) { }
}
