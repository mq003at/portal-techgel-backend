namespace portal.Mappings;

using AutoMapper;
using portal.DTOs;
using portal.Models;

public class SignatureProfile : Profile
{
    public SignatureProfile()
    {
        CreateMap<Signature, SignatureDTO>()
            .ForMember(dest => dest.FileUrl, opt => opt.MapFrom(src => src.StoragePath));

        CreateMap<UploadSignatureDTO, Signature>()
            .ForMember(dest => dest.ContentType, opt => opt.MapFrom(src => src.File.ContentType))
            .ForMember(dest => dest.FileSize, opt => opt.MapFrom(src => src.File.Length))
            .ForMember(dest => dest.StoragePath, opt => opt.Ignore())
            .ForMember(dest => dest.UploadedAt, opt => opt.Ignore());

        CreateMap<UpdateSignatureDTO, Signature>()
            .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));
    }
}
