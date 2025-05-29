using portal.DTOs;
using portal.Enums;
using portal.Models;

namespace portal.Services;

public interface IApprovalWorkflowNodeService
    : IBaseService<
        ApprovalWorkflowNode,
        ApprovalWorkflowNodeDTO,
        CreateApprovalWorkflowNodeDTO,
        UpdateApprovalWorkflowNodeDTO
    >
{
    Task<ApprovalWorkflowNodeDTO> UpdateRelatedDocument(List<int> documentIds, int nodeId);
    Task<string> SignDocumentByUpdatingTheDocumentAsync(
        int nodeId,
        int DocumentId,
        Stream file,
        string fileName
    );
    Task<GeneralWorkflowStatusType> CheckApprovalStatusAsync(int nodeId);
    Task UploadSupportingDocumentsAsync(int nodeId, List<IFormFile> files);
}
