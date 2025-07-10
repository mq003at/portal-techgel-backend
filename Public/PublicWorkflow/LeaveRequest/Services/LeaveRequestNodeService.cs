namespace portal.Services;

using AutoMapper;
using DotNetCore.CAP;
using portal.Db;
using portal.DTOs;
using portal.Models;

public class LeaveRequestNodeService
    : BaseNodeService<
        LeaveRequestNode,
        LeaveRequestNodeDTO,
        LeaveRequestNodeCreateDTO,
        LeaveRequestNodeUpdateDTO,
        LeaveRequestWorkflow
    >,
        ILeaveRequestNodeService
{
    public LeaveRequestNodeService(
        ApplicationDbContext context,
        IMapper mapper,
        ILogger<LeaveRequestNodeService> logger,
        ICapPublisher capPublisher
    )
        : base(context, mapper, logger, capPublisher) { }
}
