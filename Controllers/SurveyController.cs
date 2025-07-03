namespace portal.Controllers;

using Microsoft.AspNetCore.Mvc;
using portal.DTOs;
using portal.Models;
using portal.Services;

public class SurveyController : BaseController<Survey, SurveyDTO, SurveyCreateDTO, SurveyUpdateDTO>
{
    private readonly ISurveyService _surveyService;

    public SurveyController(ISurveyService service)
        : base(service)
    {
        _surveyService = service;
    }
}
