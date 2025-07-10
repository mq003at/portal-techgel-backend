using Microsoft.AspNetCore.Mvc;
using portal.DTOs;
using portal.Models;
using portal.Services;

namespace portal.Controllers;

[ApiController]
[Route("api/gate-pass-nodes")]
public class GatePassNodeController
    : BaseNodeController<
        GatePassNode,
        GatePassNodeDTO,
        GatePassNodeCreateDTO,
        GatePassNodeUpdateDTO,
        GatePassWorkflow
    >
{
    public GatePassNodeController(IGatePassNodeService nodeService)
        : base(nodeService) { }
}
