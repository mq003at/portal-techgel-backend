using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using portal.Db;
using portal.DTOs;
using portal.Enums;
using portal.Models;

namespace portal.Services;

public class ApprovalWorkflowNodeService : IApprovalWorkflowNodeService
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger<ApprovalWorkflowNodeService> _logger;
    private readonly IDocumentService _documentService;
    private readonly DocumentOptions _docOpts;
    private readonly IFileStorageService _storage;

    public ApprovalWorkflowNodeService(
        ApplicationDbContext context,
        IMapper mapper,
        IDocumentService documentService,
        ILogger<ApprovalWorkflowNodeService> logger,
        IOptions<DocumentOptions> docOpts,
        IFileStorageService _storage
    )
    {
        _context = context;
        _mapper = mapper;
        _documentService = documentService;
        _logger = logger;
        _docOpts = docOpts.Value;
        _storage = _storage;
    }

    public async Task<ApprovalWorkflowNodeFileResultDTO> AttachDocumentsToNodeAsync(
        UpdateFilesInApprovalWorkflowNodesDTO dto
    )
    

    }

    public async Task DeleteFilesFromNodeAsync(DeleteFilesFromApprovalWorkflowNodesDTO dto, int id)
    {
        if (dto.DocumentIds == null || !dto.DocumentIds.Any())
            throw new ArgumentException("No document IDs provided.");

        var node = await _context
            .ApprovalWorkflowNodes.Include(n => n.WorkflowNodeDocuments)
            .FirstOrDefaultAsync(n => n.Id == id);

        if (node is null)
            throw new KeyNotFoundException($"ApprovalWorkflowNode with ID {id} not found.");

        foreach (var docId in dto.DocumentIds)
        {
            var document = await _context.Documents.FindAsync(docId);
            if (document is null)
                continue;

            var category = document.GeneralDocumentInfo.Category.ToString();
            var name = document.GeneralDocumentInfo.Name;
            var relPath = $"{category}/{name}";
            var fullPath = $"{_docOpts.StorageDir.TrimEnd('/')}/{relPath}";

            if (await _storage.ExistsAsync(fullPath))
            {
                await _storage.DeleteAsync(fullPath);
            }

            // Remove link from join table
            var link = node.WorkflowNodeDocuments.FirstOrDefault(w => w.DocumentId == docId);
            if (link is not null)
                _context.Remove(link);

            _context.Documents.Remove(document);
        }

        await _context.SaveChangesAsync();
    }
}
