using Microsoft.AspNetCore.Mvc;
using portal.DTOs;
using portal.Models;
using portal.Services;

namespace portal.Controllers;
[Route("api/[controller]s")]
public class GeneralWorkflowController
    : BaseController<
        GeneralWorkflow,
        GeneralWorkflowDTO,
        CreateGeneralWorkflowDTO,
        UpdateGeneralWorkflowDTO
    >
{
    public GeneralWorkflowController(
        IGeneralWorkflowService workflowService
    )
        : base(workflowService)
    {

    }
}
