// using AutoMapper;
// using Microsoft.EntityFrameworkCore;
// using Microsoft.Extensions.Options;
// using portal.Db;
// using portal.DTOs;
// using portal.Enums;
// using portal.Models;

// namespace portal.Services;

// public class ApprovalWorkflowNodeService
//     : BaseService<
//         ApprovalWorkflowNode,
//         ApprovalWorkflowNodeDTO,
//         CreateApprovalWorkflowNodeDTO,
//         UpdateApprovalWorkflowNodeDTO
//     >,
//         IApprovalWorkflowNodeService
// {
//     private readonly DbSet<ApprovalWorkflowNode> _approvalWorkflowNodes;
//     private readonly DbSet<Document> _documents;
//     private readonly IDocumentService _documentService;
//     private readonly DocumentOptions _docOpts;
//     private readonly IFileStorageService _storage;

//     public ApprovalWorkflowNodeService(
//         ApplicationDbContext context,
//         IMapper mapper,
//         ILogger<ApprovalWorkflowNodeService> logger,
//         IDocumentService documentService,
//         IFileStorageService storage,
//         IOptions<DocumentOptions> docOpts
//     )
//         : base(context, mapper, logger)
//     {
//         _approvalWorkflowNodes = context.Set<ApprovalWorkflowNode>();
//         _documents = context.Set<Document>();
//         _documentService = documentService;
//         _docOpts = docOpts.Value;
//         _storage = storage;
//     }

//     public override async Task<ApprovalWorkflowNodeDTO> CreateAsync(
//         CreateApprovalWorkflowNodeDTO dto
//     )
//     {
//         var nodeDto = await base.CreateAsync(dto);
//         await PopulateNamesAndDocsAsync(nodeDto);

//         var general = await _context.Set<GeneralWorkflow>()
//     .FirstOrDefaultAsync(g => g.Id == nodeDto.GeneralWorkflowId);

//         if (general != null)
//         {
//             if (!general.ApprovalWorkflowNodesIds.Contains((int)nodeDto.Id))
//                 general.ApprovalWorkflowNodesIds.Add((int)nodeDto.Id);
//             _context.Update(general);
//             await _context.SaveChangesAsync();
//         }

//         return nodeDto;
//     }

//     public override async Task<ApprovalWorkflowNodeDTO?> GetByIdAsync(int id)
//     {
//         var dto = await base.GetByIdAsync(id);
//         if (dto != null)
//             await PopulateNamesAndDocsAsync(dto);
//         return dto;
//     }

//     public override async Task<IEnumerable<ApprovalWorkflowNodeDTO>> GetAllAsync()
//     {
//         var list = (await base.GetAllAsync()).ToList();
//         foreach (var dto in list)
//             await PopulateNamesAndDocsAsync(dto);
//         return list;
//     }

//     public override async Task<ApprovalWorkflowNodeDTO> UpdateAsync(
//         int id,
//         UpdateApprovalWorkflowNodeDTO dto
//     )
//     {
//         var nodeDto = await base.UpdateAsync(id, dto);
//         if (nodeDto != null)
//             await PopulateNamesAndDocsAsync(nodeDto);

//         var general = await _context.Set<GeneralWorkflow>()
//     .FirstOrDefaultAsync(g => g.Id == nodeDto.GeneralWorkflowId);
//         if (general != null)
//         {
//             general.UpdatedAt = DateTime.UtcNow;
//             _logger.LogInformation(
//                 "Updating GeneralWorkflow {Id} with new node {NodeId}",
//                 general.Id,
//                 nodeDto.Id
//             );
//             if (!general.ApprovalWorkflowNodesIds.Contains((int)nodeDto.Id))
//                 general.ApprovalWorkflowNodesIds.Add((int)nodeDto.Id);
//             _context.Update(general);
//             await _context.SaveChangesAsync();
//         }
//         return nodeDto;
//     }

//     public async Task<ApprovalWorkflowNodeDTO> UpdateRelatedDocument(
//         List<int> documentIds,
//         int nodeId
//     )
//     {
//         var node =
//             await _context.ApprovalWorkflowNodes.FindAsync(nodeId)
//             ?? throw new KeyNotFoundException($"Node {nodeId} not found.");

//         node.DocumentIds = documentIds;
//         _context.ApprovalWorkflowNodes.Update(node);
//         await _context.SaveChangesAsync();

//         return _mapper.Map<ApprovalWorkflowNodeDTO>(node);
//     }

//     public async Task<string> SignDocumentByUpdatingTheDocumentAsync(
//         int nodeId,
//         int documentId,
//         Stream file,
//         string fileName
//     )
//     {
//         var node =
//             await _context.ApprovalWorkflowNodes.FindAsync(nodeId)
//             ?? throw new KeyNotFoundException($"Node {nodeId} not found.");

//         var document =
//             await _context.Documents.FindAsync(documentId)
//             ?? throw new KeyNotFoundException($"Document {documentId} not found.");

//         var category = document.GeneralDocumentInfo.Category.ToString();
//         var path = $"{_docOpts.StorageDir.TrimEnd('/')}/{category}/{fileName}";

//         if (await _storage.Exists(path))
//         {
//             await _storage.DeleteAsync(path);
//         }

//         await _storage.UploadAsync(file, path);

//         document.GeneralDocumentInfo.Url =
//             $"{_docOpts.PublicBaseUrl.TrimEnd('/')}/{category}/{fileName}";
//         _context.Documents.Update(document);
//         await _context.SaveChangesAsync();

//         return document.GeneralDocumentInfo.Url;
//     }

//     public async Task<GeneralWorkflowStatusType> CheckApprovalStatusAsync(int nodeId)
//     {
//         var node =
//             await _context.ApprovalWorkflowNodes.FindAsync(nodeId)
//             ?? throw new KeyNotFoundException($"Node {nodeId} not found.");
//         return node.Status;
//     }

//     public async Task UploadSupportingDocumentsAsync(int nodeId, List<IFormFile> files)
//     {
//         var node =
//             await _context.ApprovalWorkflowNodes.FindAsync(nodeId)
//             ?? throw new KeyNotFoundException($"Node {nodeId} not found.");

//         foreach (var file in files)
//         {
//             var fileName = file.FileName;
//             var category = "ARCHIVE";
//             var relPath = $"{category}/{fileName}";
//             var fullPath = $"{_docOpts.StorageDir.TrimEnd('/')}/{relPath}";

//             if (await _storage.Exists(fullPath))
//                 throw new InvalidOperationException($"File {fileName} already exists on server.");

//             await using var stream = file.OpenReadStream();
//             await _storage.UploadAsync(stream, fullPath);

//             var doc = new CreateDocumentDTO
//             {
//                 // GeneralDocumentInfo = new GeneralDocumentInfoDTO
//                 // {
//                 //     Name = fileName,
//                 //     Category = DocumentCategoryEnum.ARCHIVE,
//                 //     Url = $"{_docOpts.PublicBaseUrl.TrimEnd('/')}/{relPath}"
//                 // }
//             };

//             var created = await _documentService.CreateMetaDataAsync(doc);
//             var createdId = created?.Id;
//             if (!createdId.HasValue)
//                 throw new InvalidOperationException("Failed to create document metadata.");
//             else
//                 node.DocumentIds.Add(createdId.Value);
//         }

//         _context.ApprovalWorkflowNodes.Update(node);
//         await _context.SaveChangesAsync();
//     }

//     public async Task PopulateNamesAndDocsAsync(ApprovalWorkflowNodeDTO nodeDto)
//     {
//         // Populate SenderName
//         var sender = await _context.Set<Employee>().FindAsync(nodeDto.SenderId);
//         nodeDto.SenderName = sender != null
//             ? $"{sender.FirstName} {sender.LastName}".Trim()
//             : null;

//         // Populate ReceiverNames
//         if (nodeDto.ReceiverIds != null && nodeDto.ReceiverIds.Count > 0)
//         {
//             var receivers = await _context.Set<Employee>()
//                 .Where(e => nodeDto.ReceiverIds.Contains(e.Id))
//                 .ToListAsync();

//             nodeDto.ReceiverNames = receivers
//                 .OrderBy(e => nodeDto.ReceiverIds.IndexOf(e.Id))
//                 .Select(e => $"{e.FirstName} {e.LastName}".Trim())
//                 .ToList();
//         }

//         // Populate DocumentNames and (optionally) URLs
//         if (nodeDto.DocumentIds != null && nodeDto.DocumentIds.Count > 0)
//         {
//             var docIdList = nodeDto.DocumentIds.ToList();

//             var documents = await _context.Set<Document>()
//                 .Where(d => docIdList.Contains(d.Id))
//                 .ToListAsync();

//             nodeDto.DocumentNames = documents
//                 .OrderBy(d => docIdList.IndexOf(d.Id))
//                 .Select(d => d.GeneralDocumentInfo.Name)
//                 .ToList();

//             _logger.LogInformation(
//                 "docId: {id}, docNames: {names}",
//                 nodeDto.DocumentIds.Count,
//                 nodeDto.DocumentNames.Count
//             );
//         }
//     }
// }
