namespace portal.Controllers;

using portal.DTOs;
using portal.Models;
using portal.Services;

public class TeamController : BaseController<Team, TeamDTO, UpdateTeamDTO>
{
    public TeamController(ITeamService TeamService)
        : base(TeamService) { }
}
