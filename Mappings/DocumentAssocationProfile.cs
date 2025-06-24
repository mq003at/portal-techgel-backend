using AutoMapper;
using portal.DTOs;
using portal.Models;

namespace portal.Mappings
{
    public class DocumentAssociationProfile : Profile
    {
        public DocumentAssociationProfile()
        {
            // CreateMap<DocumentAssociation, DocumentAssociationDTO>()
            // .ForMember(dest => dest.DocumentName, opt => opt.MapFrom(src => src.Document.Name))
            // .ForMember(dest => dest.DocumentURL, opt => opt.MapFrom(src => src.Document.Url));

            // // Create DTO ➜ Entity
            // CreateMap<DocumentAssociationCreateDTO, DocumentAssociation>();

            // // Update DTO ➜ Entity (manual mapping recommended for partial updates, but AutoMapper fallback shown)
            // CreateMap<DocumentAssociationUpdateDTO, DocumentAssociation>()
            //     .ForAllMembers(opt => opt.Condition((src, context) => src != null));
        }
    }
}