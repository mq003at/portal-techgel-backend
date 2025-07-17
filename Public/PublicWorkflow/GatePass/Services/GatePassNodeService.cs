namespace portal.Services;

using AutoMapper;
using DotNetCore.CAP;
using portal.Db;
using portal.DTOs;
using portal.Models;

public class GatePassNodeService
    : BaseNodeService<
        GatePassNode,
        GatePassNodeDTO,
        GatePassNodeCreateDTO,
        GatePassNodeUpdateDTO,
        GatePassWorkflow
    >,
        IGatePassNodeService
{
    public GatePassNodeService(
        ApplicationDbContext context,
        IMapper mapper,
        ILogger<GatePassNodeService> logger
    )
        : base(context, mapper, logger) { }
}
