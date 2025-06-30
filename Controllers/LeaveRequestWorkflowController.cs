using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using portal.DTOs;
using portal.Models;
using portal.Services;

namespace portal.Controllers;

[ApiController]
[Route("api/leave-requests")]
public class LeaveRequestWorkflowController
    : BaseController<
        LeaveRequestWorkflow,
        LeaveRequestWorkflowDTO,
        LeaveRequestWorkflowCreateDTO,
        LeaveRequestWorkflowUpdateDTO
    >
{
    private readonly ILeaveRequestWorkflowService _workflowService;

    public LeaveRequestWorkflowController(ILeaveRequestWorkflowService workflowService)
        : base(workflowService)
    {
        _workflowService = workflowService;
    }

    // Tắt bớt các route nếu không cần → giữ nguyên GetAll, GetById, Create, Update, Delete
    // GET api/leave-requests/{id}/nodes
    [HttpGet("{id}/nodes")]
    public async Task<IActionResult> GetNodes(int id)
    {
        var nodes = await _workflowService.GetNodesByWorkflowIdAsync(id);
        return Ok(nodes);
    }

    [HttpGet]
    [Authorize]
    public override async Task<ActionResult<IEnumerable<LeaveRequestWorkflowDTO>>> GetAll()
    {
        // If user is HR
        string claimValue =
            User.FindFirst("OrganizationEntityIds")?.Value
            ?? throw new UnauthorizedAccessException("OrganizationEntityIds claim not found.");
        string idClaim =
            User.FindFirst("Id")?.Value
            ?? throw new UnauthorizedAccessException("Id claim not found.");
        int id = int.Parse(idClaim);
        var organizationIds = claimValue
            .Split(",", StringSplitOptions.RemoveEmptyEntries)
            .Select(id => int.Parse(id))
            .ToList();

        var targetIds = new List<int> { 11, 3 };
        if (organizationIds.Any(targetIds.Contains))
            return await base.GetAll();
        else
        {
            LeaveRequestWorkflowDTO workflow =
                await _workflowService.GetByIdAsync(id)
                ?? throw new Exception("Workflow not found.");
            return new List<LeaveRequestWorkflowDTO> { workflow };
        }
    }

    // replace get with this one. If else for me. Check OrganizationEntityEmployees to see if this employeeId has access to organizationEntityId = 11
}
