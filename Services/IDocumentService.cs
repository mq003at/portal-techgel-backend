using portal.DTOs;
using portal.Enums;

namespace portal.Services;

public interface IDocumentService
{
    // Generic CRUD
    Task<DocumentDTO> CreateMetaDataAsync(CreateDocumentDTO dto);
    Task<DocumentDTO> UpdateAsync(int id, UpdateDocumentDTO dto);
    Task<bool> DeleteAsync(int id);
    Task<DocumentDTO?> GetByIdAsync(int id);
    Task<IEnumerable<DocumentDTO>> GetAllMetaDataAsync();

    // File operations
    Task<DocumentDTO> UploadDocumentAsync(CreateDocumentDTO dto);
    Task<DocumentDTO> UploadAndReplaceDocumentAsync(UpdateDocumentDTO dto, int id);
    Task<bool> IsFileExistAsync(string category, string fileName);
    Task<DocumentStatusEnum> CheckDocumentStatusAsync(int id);
}
