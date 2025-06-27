namespace portal.Services;

using portal.DTOs;
using portal.Models;
public interface ILeaveRequestNodeService
    : IBaseService<
        LeaveRequestNode,
        LeaveRequestNodeDTO,
        LeaveRequestNodeCreateDTO,
        LeaveRequestNodeUpdateDTO>
{
    Task<string> ApproveAsync(int nodeId, int approverId);
    Task<string> RejectAsync(int nodeId, int approverId, string rejectReason);

}
