using Microsoft.AspNetCore.Mvc;
using portal.DTOs;
using portal.Models;
using portal.Services;

namespace portal.Controllers;

[ApiController]
[Route("api/leave-request-nodes")]
public class LeaveRequestNodeController
    : BaseNodeController<
        LeaveRequestNode,
        LeaveRequestNodeDTO,
        LeaveRequestNodeCreateDTO,
        LeaveRequestNodeUpdateDTO,
        LeaveRequestWorkflow
    >
{
    public LeaveRequestNodeController(
        IBaseNodeService<
            LeaveRequestNode,
            LeaveRequestNodeDTO,
            LeaveRequestNodeCreateDTO,
            LeaveRequestNodeUpdateDTO,
            LeaveRequestWorkflow
        > nodeService
    )
        : base(nodeService) { }
}
