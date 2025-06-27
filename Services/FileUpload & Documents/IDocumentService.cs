using portal.DTOs;
using portal.Enums;

namespace portal.Services;

public interface IDocumentService
{
    // Generic CRUD for metadata only
    Task<DocumentDTO> CreateMetaDataAsync(DocumentCreateDTO dto);
    Task<DocumentDTO> UpdateMetaDataAsync(int id, DocumentUpdateDTO dto);
    Task<DocumentDTO?> GetMetaDataByIdAsync(int id);
    Task<IEnumerable<DocumentDTO>> GetAllMetaDataAsync();
    Task<bool> DeleteMetaDataAsync(int id);


    // File operations
    Task<DocumentDTO> UploadDocumentAsync(DocumentCreateDTO dto);
    Task<DocumentDTO> UploadAndReplaceDocumentAsync(DocumentUpdateDTO dto, int id);
    Task<bool> IsFileExistAsync(string category, string fileName);
    Task<bool> IsUrlAccessibleAsync(string url);
    Task<DocumentStatusEnum> CheckDocumentSignStatusAsync(int id);
    Task<bool> SignFileAsync(int documentId, Stream signedFileStream);
    Task<List<DocumentDTO>> SearchByTagsAsync(List<string> tags);


    //Template handling
    Task<DocumentDTO> GetTemplateAsync(string templateKey);
    Task<DocumentDTO> UpdateTemplateAsync(DocumentUpdateDTO dto);
    Task<DocumentDTO> FillInTemplateAsync(FillInTemplateDTO dto);
    Task<DocumentDTO> CreateTemplateAsync(DocumentTemplateCreateDTO dto);
}
