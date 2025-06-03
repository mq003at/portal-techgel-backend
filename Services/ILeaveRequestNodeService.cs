namespace portal.Services;

using portal.DTOs;
using portal.Models;
public interface ILeaveRequestNodeService
    : IBaseService<
        LeaveRequestNode,
        LeaveRequestNodeDTO,
        CreateLeaveRequestNodeDTO,
        UpdateLeaveRequestNodeDTO>
{
    Task<bool> ApproveAsync(int nodeId, int approverId, string? comment = null);
}
