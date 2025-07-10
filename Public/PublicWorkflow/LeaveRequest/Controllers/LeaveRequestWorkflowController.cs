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

    [HttpDelete("{id}")]
    [Authorize]
    public override async Task<ActionResult> Delete(int id)
    {
        string orgEntClaim =
            User.FindFirst("OrganizationEntityIds")?.Value
            ?? throw new UnauthorizedAccessException(
                "Lỗi chứng thực người dùng. Xin đăng xuất và đăng nhập lại."
            );
        string idClaim =
            User.FindFirst("Id")?.Value
            ?? throw new UnauthorizedAccessException(
                "Lỗi không nhận diện được người dùng này. Vui lòng đăng nhập lại."
            );

        var organizationIds = orgEntClaim
            .Split(",", StringSplitOptions.RemoveEmptyEntries)
            .Select(int.Parse)
            .ToList();

        var targetIds = new List<int> { 3, 10, 11, 12, 13, 64 };
        if (organizationIds.Any(targetIds.Contains) || int.Parse(idClaim) == id)
            return await base.Delete(id);
        else
        {
            return Unauthorized("Người dùng không có thẩm quyền xóa.");
        }
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

        var targetIds = new List<int> { 3, 10, 11, 12, 13, 64 };
        if (organizationIds.Any(targetIds.Contains))
            return await base.GetAll();
        else
        {
            List<LeaveRequestWorkflowDTO> workflows =
                await _workflowService.GetAllByEmployeeIdAsync(id)
                ?? throw new Exception("Workflow not found.");
            return workflows;
        }
    }

    [HttpPost("{id}/generate-documents")]
    [Authorize]
    public async Task<IActionResult> GenerateDocuments(int id)
    {
        if (id < 0)
            return BadRequest("Lỗi ID.");

        var result = await _workflowService.FinalizeIfCompleteAsync(id);
        if (result == false)
            return NotFound(
                "Không có tài liệu nào được tạo cho ID quy trình làm việc đã cung cấp."
            );

        return Ok(true);
    }
}
