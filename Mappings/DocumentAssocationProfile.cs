using AutoMapper;
using portal.DTOs;
using portal.Models;

namespace portal.Mappings
{
    public class DocumentAssociationProfile : Profile
    {
        public DocumentAssociationProfile()
        {
            CreateMap<DocumentAssociation, DocumentAssociationDTO>()
                .ForMember(dest => dest.DocumentName, opt => opt.MapFrom(src => src.Document.Name))
                .ForMember(dest => dest.DocumentId, opt => opt.MapFrom(src => src.DocumentId))
                .ForMember(dest => dest.EntityType, opt => opt.MapFrom(src => src.EntityType))
                .ReverseMap()
                .ForMember(dest => dest.Document, opt => opt.Ignore()); // prevent circular or unwanted remap
        }
    }
}