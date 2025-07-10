namespace portal.Services;

using portal.DTOs;
using portal.Models;

public interface ILeaveRequestNodeService
    : IBaseNodeService<
        LeaveRequestNode,
        LeaveRequestNodeDTO,
        LeaveRequestNodeCreateDTO,
        LeaveRequestNodeUpdateDTO,
        LeaveRequestWorkflow
    > { }
