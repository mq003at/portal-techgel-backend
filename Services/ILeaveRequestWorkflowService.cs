namespace portal.Services;

using portal.DTOs;
using portal.Models;

public interface ILeaveRequestWorkflowService
    : IBaseService<
        LeaveRequestWorkflow,
        LeaveRequestWorkflowDTO,
        CreateLeaveRequestWorkflowDTO,
        UpdateLeaveRequestWorkflowDTO>
{
    /// <summary>
    /// Tự động sinh các bước duyệt (nodes) dựa vào số ngày nghỉ
    /// </summary>
    Task<ICollection<LeaveRequestNodeDTO>> GenerateStepsAsync(CreateLeaveRequestWorkflowDTO dto, int workflowId);

    /// <summary>
    /// Xử lý duyệt 1 node cụ thể (ký, ghi nhận người duyệt)
    /// </summary>
    Task<bool> ApproveNodeAsync(int nodeId, int approverId, string? comment = null);

    /// <summary>
    /// Lấy tất cả node theo Workflow ID
    /// </summary>
    Task<IEnumerable<LeaveRequestNodeDTO>> GetNodesByWorkflowIdAsync(int workflowId);

    /// <summary>
    /// Gán trạng thái hoàn tất nếu tất cả step đã hoàn thành
    /// </summary>
    Task<bool> FinalizeIfCompleteAsync(int workflowId);
}