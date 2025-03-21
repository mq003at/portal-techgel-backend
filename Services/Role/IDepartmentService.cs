namespace portal.Services;

using portal.DTOs;
using portal.Models;

public interface IDepartmentService
    : IBaseService<Department, DepartmentDTO, UpdateDepartmentDTO> { }
