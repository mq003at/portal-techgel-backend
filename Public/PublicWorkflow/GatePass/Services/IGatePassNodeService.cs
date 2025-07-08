using portal.DTOs;
using portal.Models;

namespace portal.Services;

public interface IGatePassNodeService
    : IBaseNodeService<
        GatePassNode,
        GatePassNodeDTO,
        GatePassNodeCreateDTO,
        GatePassNodeUpdateDTO,
        GatePassWorkflow
    > { }
