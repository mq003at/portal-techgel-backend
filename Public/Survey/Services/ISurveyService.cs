namespace portal.Services;

using portal.DTOs;
using portal.Models;

public interface ISurveyService
    : IBaseService<Survey, SurveyDTO, SurveyCreateDTO, SurveyUpdateDTO> { }

public interface ISurveyResponseService
    : IBaseModelWithOnlyIdService<
        SurveyResponse,
        SurveyResponseDTO,
        SurveyResponseCreateDTO,
        SurveyResponseUpdateDTO
    > { }
