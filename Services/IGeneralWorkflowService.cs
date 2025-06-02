using portal.DTOs;
using portal.Models;

namespace portal.Services;

public interface IGeneralWorkflowService
    : IBaseService<
        GeneralWorkflow,
        GeneralWorkflowDTO,
        CreateGeneralWorkflowDTO,
        UpdateGeneralWorkflowDTO
    >
{
}
