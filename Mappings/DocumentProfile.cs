using System.Globalization;

namespace portal.Mappings;

using AutoMapper;
using portal.DTOs;
using portal.Helpers;
using portal.Models;

public class DocumentProfile : Profile
{
    public DocumentProfile()
    {
        // ----- NESTED/OWNED TYPE MAPPINGS (skip nulls on update) -----
        CreateMap<GeneralDocumentInfo, GeneralDocumentInfoDTO>().ReverseMap();
        CreateMap<CreateGeneralDocumentInfoDTO, GeneralDocumentInfo>().ReverseMap();
        CreateMap<UpdateGeneralDocumentInfoDTO, GeneralDocumentInfo>()
            .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

        CreateMap<LegalDocumentInfo, LegalDocumentInfoDTO>().ReverseMap();
        CreateMap<CreateLegalDocumentInfoDTO, LegalDocumentInfo>().ReverseMap();
        CreateMap<UpdateLegalDocumentInfoDTO, LegalDocumentInfo>()
            .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

        CreateMap<SecurityDocumentInfo, SecurityDocumentInfoDTO>().ReverseMap();
        CreateMap<AdditionalDocumentInfo, AdditionalDocumentInfoDTO>().ReverseMap();
        CreateMap<EditDocumentInfo, EditDocumentInfoDTO>().ReverseMap();
        // Add similar update DTO → entity mappings if you PATCH these nested types

        // ----- DOCUMENT <-> DOCUMENTDTO -----
        CreateMap<Document, DocumentDTO>()
            .ForMember(d => d.File, o => o.Ignore())
            .ForMember(d => d.GeneralDocumentInfo, o => o.MapFrom(s => s.GeneralDocumentInfo))
            .ForMember(d => d.LegalDocumentInfo, o => o.MapFrom(s => s.LegalDocumentInfo))
            .ForMember(d => d.SecurityDocumentInfo, o => o.MapFrom(s => s.SecurityDocumentInfo))
            .ForMember(d => d.AdditionalDocumentInfo, o => o.MapFrom(s => s.AdditionalDocumentInfo))
            .ForMember(d => d.EditDocumentInfo, o => o.MapFrom(s => s.EditDocumentInfo));

        // ----- CREATEDOCUMENTDTO -> DOCUMENT -----
        CreateMap<CreateDocumentDTO, Document>()
            .ForMember(dest => dest.Id, o => o.Ignore())
            .ForMember(dest => dest.MainId, o => o.Ignore())
            .ForMember(dest => dest.CreatedAt, o => o.Ignore())
            .ForMember(dest => dest.UpdatedAt, o => o.Ignore())
            .ForMember(d => d.GeneralDocumentInfo, o => o.MapFrom(s => s.GeneralDocumentInfo))
            .ForMember(d => d.LegalDocumentInfo, o => o.MapFrom(s => s.LegalDocumentInfo))
            .ForMember(d => d.SecurityDocumentInfo, o => o.MapFrom(s => s.SecurityDocumentInfo))
            .ForMember(d => d.AdditionalDocumentInfo, o => o.MapFrom(s => s.AdditionalDocumentInfo))
            .ForMember(d => d.EditDocumentInfo, o => o.MapFrom(s => s.EditDocumentInfo));

        // ----- UPDATEDOCUMENTDTO -> DOCUMENT (skip null nested types) -----
        CreateMap<UpdateDocumentDTO, Document>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(d => d.GeneralDocumentInfo, o =>
                o.Condition((src, dest, srcMember) => src.GeneralDocumentInfo != null)
            )
            .ForMember(d => d.GeneralDocumentInfo, o =>
                o.MapFrom(s => s.GeneralDocumentInfo)
            )
            .ForMember(d => d.LegalDocumentInfo, o =>
                o.Condition((src, dest, srcMember) => src.LegalDocumentInfo != null)
            )
            .ForMember(d => d.LegalDocumentInfo, o =>
                o.MapFrom(s => s.LegalDocumentInfo)
            )
            .ForMember(d => d.SecurityDocumentInfo, o =>
                o.Condition((src, dest, srcMember) => src.SecurityDocumentInfo != null)
            )
            .ForMember(d => d.SecurityDocumentInfo, o =>
                o.MapFrom(s => s.SecurityDocumentInfo)
            )
            .ForMember(d => d.AdditionalDocumentInfo, o =>
                o.Condition((src, dest, srcMember) => src.AdditionalDocumentInfo != null)
            )
            .ForMember(d => d.AdditionalDocumentInfo, o =>
                o.MapFrom(s => s.AdditionalDocumentInfo)
            )
            .ForMember(d => d.EditDocumentInfo, o =>
                o.Condition((src, dest, srcMember) => src.EditDocumentInfo != null)
            )
            .ForMember(d => d.EditDocumentInfo, o =>
                o.MapFrom(s => s.EditDocumentInfo)
            )
            // Scalar props: do not map nulls
            .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));
    }
}
