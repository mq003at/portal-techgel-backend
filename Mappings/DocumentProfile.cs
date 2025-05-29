using System.Globalization;

namespace portal.Mappings;

using AutoMapper;
using portal.DTOs;
using portal.Models;

public class DocumentProfile : Profile
{
    public DocumentProfile()
    {
        // Entity -> DTO
        CreateMap<Document, DocumentDTO>()
            .IncludeBase<BaseModel, BaseDTO<Document>>()
            .ForMember(dest => dest.File, opt => opt.Ignore())
            .ForMember(
                dest => dest.GeneralDocumentInfo,
                opt => opt.MapFrom(src => src.GeneralDocumentInfo)
            )
            .ForMember(
                dest => dest.LegalDocumentInfo,
                opt => opt.MapFrom(src => src.LegalDocumentInfo)
            )
            .ForMember(
                dest => dest.SecurityDocumentInfo,
                opt => opt.MapFrom(src => src.SecurityDocumentInfo)
            )
            .ForMember(
                dest => dest.AdditionalDocumentInfo,
                opt => opt.MapFrom(src => src.AdditionalDocumentInfo)
            )
            .ForMember(
                dest => dest.EditDocumentInfo,
                opt => opt.MapFrom(src => src.EditDocumentInfo)
            );

        // DTO -> Entity for Create
        CreateMap<CreateDocumentDTO, Document>()
            .ForMember(dest => dest.Id, o => o.Ignore())
            .ForMember(dest => dest.MainId, o => o.Ignore())
            .ForMember(dest => dest.CreatedAt, o => o.Ignore())
            .ForMember(dest => dest.UpdatedAt, o => o.Ignore());

        // DTO -> Entity for Update
        CreateMap<UpdateDocumentDTO, Document>()
            .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

        // Owned types mapping
        CreateMap<GeneralDocumentInfo, GeneralDocumentInfoDTO>()
            .ReverseMap();
        CreateMap<SecurityDocumentInfo, SecurityDocumentInfoDTO>().ReverseMap();
        CreateMap<AdditionalDocumentInfo, AdditionalDocumentInfo>().ReverseMap();
        CreateMap<EditDocumentInfo, EditDocumentInfoDTO>().ReverseMap();

        // LegalDocumentInfo: string <-> string (no conversion needed)
        CreateMap<LegalDocumentInfo, LegalDocumentInfoDTO>()
            .ReverseMap();
    }
}
