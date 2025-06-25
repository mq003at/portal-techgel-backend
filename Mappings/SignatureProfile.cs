namespace portal.Mappings;

using AutoMapper;
using portal.DTOs;
using portal.Models;

public class SignatureProfile : BaseModelProfile<
    Signature,
    SignatureDTO,
    UploadSignatureDTO,
    UpdateSignatureDTO>
{
    public SignatureProfile()
    {
        // Additional custom mapping for UploadSignatureDTO to Signature
        CreateMap<UploadSignatureDTO, Signature>()
            .ForMember(dest => dest.FileName, opt => opt.MapFrom(src => src.FileName))
            .ForMember(dest => dest.ContentType, opt => opt.MapFrom(src => src.File.ContentType))
            .ForMember(dest => dest.FileSize, opt => opt.MapFrom(src => src.File.Length))
            .ForMember(dest => dest.StoragePath, opt => opt.Ignore()) // To be set in service
            .ForMember(dest => dest.UploadedAt, opt => opt.Ignore())  // Set in service

            // BaseModel fields
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());

        // UpdateSignatureDTO → Signature (partial update)
        CreateMap<UpdateSignatureDTO, Signature>()
            .ForMember(dest => dest.FileName, opt => opt.Ignore())      // Not in update
            .ForMember(dest => dest.ContentType, opt => opt.Ignore())   // Set in service if file uploaded
            .ForMember(dest => dest.FileSize, opt => opt.Ignore())      // Same
            .ForMember(dest => dest.StoragePath, opt => opt.Ignore())   // Same
            .ForMember(dest => dest.UploadedAt, opt => opt.Ignore())    // Same

            // BaseModel fields
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());
    }
}