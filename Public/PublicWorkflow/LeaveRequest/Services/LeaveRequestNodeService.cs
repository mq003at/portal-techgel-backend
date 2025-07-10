namespace portal.Services;

using AutoMapper;
using Microsoft.EntityFrameworkCore;
using portal.Db;
using portal.DTOs;
using portal.Enums;
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
        ILogger<
            BaseNodeService<
                LeaveRequestNode,
                LeaveRequestNodeDTO,
                LeaveRequestNodeCreateDTO,
                LeaveRequestNodeUpdateDTO,
                LeaveRequestWorkflow
            >
        > logger
    )
        : base(context, mapper, logger) { }
}
