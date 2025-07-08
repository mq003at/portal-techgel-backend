namespace portal.Mappings;

using AutoMapper;
using portal.DTOs;
using portal.Extensions;
using portal.Models;

public class SurveyResponseProfile
    : BaseModelWithOnlyIdProfile<
        SurveyResponse,
        SurveyResponseDTO,
        SurveyResponseCreateDTO,
        SurveyResponseUpdateDTO
    > { }

public class SurveyQuestionProfile
    : BaseModelWithOnlyIdProfile<
        SurveyQuestion,
        SurveyQuestionDTO,
        SurveyQuestionCreateDTO,
        SurveyQuestionUpdateDTO
    >
{
    public SurveyQuestionProfile()
        : base()
    {
        // Optional: Handle nested SurveyResponses if needed
        CreateMap<SurveyQuestion, SurveyQuestionDTO>()
            .ForMember(dest => dest.SurveyResponses, opt => opt.MapFrom(src => src.SurveyResponses))
            .ReverseMap();
    }
}

public class SurveyProfile : BaseModelProfile<Survey, SurveyDTO, SurveyCreateDTO, SurveyUpdateDTO>
{
    public SurveyProfile()
    {
        // Map SenderName from Sender
        CreateMap<Survey, SurveyDTO>()
            .ForMember(
                dest => dest.SenderName,
                opt => opt.MapFrom(src => src.Sender.GetDisplayName())
            )
            .ForMember(
                dest => dest.RecipientNames,
                opt => opt.MapFrom(src => src.Recipients.Select(r => r.GetDisplayName()))
            )
            .ForMember(dest => dest.Questions, opt => opt.MapFrom(src => src.Questions));

        CreateMap<SurveyDTO, Survey>()
            .ForMember(dest => dest.Sender, opt => opt.Ignore())
            .ForMember(dest => dest.Recipients, opt => opt.Ignore())
            .ForMember(dest => dest.Questions, opt => opt.MapFrom(src => src.Questions));

        // Map Create DTO to Model
        CreateMap<SurveyCreateDTO, Survey>()
            .ForMember(dest => dest.Recipients, opt => opt.Ignore()) // will be manually added in service
            .ForMember(dest => dest.Questions, opt => opt.MapFrom(src => src.Questions));

        // Map Update DTO to Model
        CreateMap<SurveyUpdateDTO, Survey>()
            .ForMember(dest => dest.Recipients, opt => opt.Ignore()) // will be manually replaced in service
            .ForMember(dest => dest.Questions, opt => opt.MapFrom(src => src.Questions));
    }
}
