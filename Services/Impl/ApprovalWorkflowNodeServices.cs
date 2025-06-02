using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using portal.Db;
using portal.DTOs;
using portal.Enums;
using portal.Models;

namespace portal.Services;

public class ApprovalWorkflowNodeService
    : BaseService<
        ApprovalWorkflowNode,
        ApprovalWorkflowNodeDTO,
        CreateApprovalWorkflowNodeDTO,
        UpdateApprovalWorkflowNodeDTO
    >,
        IApprovalWorkflowNodeService
{
    private readonly DbSet<ApprovalWorkflowNode> _approvalWorkflowNodes;
    private readonly DbSet<Document> _documents;
    private readonly IDocumentService _documentService;
    private readonly DocumentOptions _docOpts;
    private readonly IFileStorageService _storage;

    public ApprovalWorkflowNodeService(
        ApplicationDbContext context,
        IMapper mapper,
        ILogger<ApprovalWorkflowNodeService> logger,
        IDocumentService documentService,
        IFileStorageService storage,
        IOptions<DocumentOptions> docOpts
    )
        : base(context, mapper, logger)
    {
        _approvalWorkflowNodes = context.Set<ApprovalWorkflowNode>();
        _documents = context.Set<Document>();
        _documentService = documentService;
        _docOpts = docOpts.Value;
        _storage = storage;
    }

    public async Task<ApprovalWorkflowNodeDTO> UpdateRelatedDocument(
        List<int> documentIds,
        int nodeId
    )
    {
        var node =
            await _context.ApprovalWorkflowNodes.FindAsync(nodeId)
            ?? throw new KeyNotFoundException($"Node {nodeId} not found.");

        node.DocumentIds = documentIds;
        _context.ApprovalWorkflowNodes.Update(node);
        await _context.SaveChangesAsync();

        return _mapper.Map<ApprovalWorkflowNodeDTO>(node);
    }

    public async Task<string> SignDocumentByUpdatingTheDocumentAsync(
        int nodeId,
        int documentId,
        Stream file,
        string fileName
    )
    {
        var node =
            await _context.ApprovalWorkflowNodes.FindAsync(nodeId)
            ?? throw new KeyNotFoundException($"Node {nodeId} not found.");

        var document =
            await _context.Documents.FindAsync(documentId)
            ?? throw new KeyNotFoundException($"Document {documentId} not found.");

        var category = document.GeneralDocumentInfo.Category.ToString();
        var path = $"{_docOpts.StorageDir.TrimEnd('/')}/{category}/{fileName}";

        if (await _storage.Exists(path))
        {
            await _storage.DeleteAsync(path);
        }

        await _storage.UploadAsync(file, path);

        document.GeneralDocumentInfo.Url =
            $"{_docOpts.PublicBaseUrl.TrimEnd('/')}/{category}/{fileName}";
        _context.Documents.Update(document);
        await _context.SaveChangesAsync();

        return document.GeneralDocumentInfo.Url;
    }

    public async Task<GeneralWorkflowStatusType> CheckApprovalStatusAsync(int nodeId)
    {
        var node =
            await _context.ApprovalWorkflowNodes.FindAsync(nodeId)
            ?? throw new KeyNotFoundException($"Node {nodeId} not found.");
        return node.Status;
    }

    public async Task UploadSupportingDocumentsAsync(int nodeId, List<IFormFile> files)
    {
        var node =
            await _context.ApprovalWorkflowNodes.FindAsync(nodeId)
            ?? throw new KeyNotFoundException($"Node {nodeId} not found.");

        foreach (var file in files)
        {
            var fileName = file.FileName;
            var category = "ARCHIVE";
            var relPath = $"{category}/{fileName}";
            var fullPath = $"{_docOpts.StorageDir.TrimEnd('/')}/{relPath}";

            if (await _storage.Exists(fullPath))
                throw new InvalidOperationException($"File {fileName} already exists on server.");

            await using var stream = file.OpenReadStream();
            await _storage.UploadAsync(stream, fullPath);

            var doc = new CreateDocumentDTO
            {
                // GeneralDocumentInfo = new GeneralDocumentInfoDTO
                // {
                //     Name = fileName,
                //     Category = DocumentCategoryEnum.ARCHIVE,
                //     Url = $"{_docOpts.PublicBaseUrl.TrimEnd('/')}/{relPath}"
                // }
            };

            var created = await _documentService.CreateMetaDataAsync(doc);
            var createdId = created?.Id;
            if (!createdId.HasValue)
                throw new InvalidOperationException("Failed to create document metadata.");
            else
                node.DocumentIds.Add(createdId.Value);
        }

        _context.ApprovalWorkflowNodes.Update(node);
        await _context.SaveChangesAsync();
    }
}
