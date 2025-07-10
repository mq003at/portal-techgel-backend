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
        ILogger<GatePassNodeService> logger,
        ICapPublisher capPublisher
    )
        : base(context, mapper, logger, capPublisher) { }
}
