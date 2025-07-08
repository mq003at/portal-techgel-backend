namespace portal.Services;

using AutoMapper;
using Microsoft.EntityFrameworkCore;
using portal.Db;
using portal.DTOs;
using portal.Enums;
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
        ILogger<
            BaseNodeService<
                GatePassNode,
                GatePassNodeDTO,
                GatePassNodeCreateDTO,
                GatePassNodeUpdateDTO,
                GatePassWorkflow
            >
        > logger
    )
        : base(context, mapper, logger) { }
}
