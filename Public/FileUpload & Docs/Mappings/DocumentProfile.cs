using System.Globalization;

namespace portal.Mappings;

using AutoMapper;
using portal.DTOs;
using portal.Enums;
using portal.Helpers;
using portal.Models;

public class DocumentProfile
    : BaseModelProfile<Document, DocumentDTO, DocumentCreateDTO, DocumentUpdateDTO>
{
    public DocumentProfile()
    {
        // Map Document.Signatures → List<SignaturesInDocumentDTO>
        CreateMap<Document, DocumentDTO>()
            .ReverseMap();

        // Template area
        CreateMap<DocumentTemplateCreateDTO, Document>()
            .ForMember(dest => dest.TemplateKey, opt => opt.MapFrom(src => src.TemplateKey))
            .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
            .ForMember(
                dest => dest.FileExtension,
                opt => opt.MapFrom(src => Path.GetExtension(src.File.FileName))
            )
            .ForMember(dest => dest.SizeInBytes, opt => opt.MapFrom(src => src.File.Length))
            .ForMember(dest => dest.Version, opt => opt.MapFrom(_ => "1.0"))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(_ => DocumentStatusEnum.DRAFT))
            .ForMember(dest => dest.Tag, opt => opt.MapFrom(_ => new List<string>())) // Optional fallback
            .ForMember(dest => dest.Url, opt => opt.Ignore())
            .ForMember(dest => dest.Name, opt => opt.Ignore());
    }
}
