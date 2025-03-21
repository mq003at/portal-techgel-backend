namespace portal.Controllers;

using portal.DTOs;
using portal.Models;
using portal.Services;

public class DepartmentController : BaseController<Department, DepartmentDTO, UpdateDepartmentDTO>
{
    public DepartmentController(IDepartmentService departmentService)
        : base(departmentService) { }
}
