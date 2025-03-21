namespace portal.Controllers;

using portal.DTOs;
using portal.Models;
using portal.Services;

public class EmployeeController : BaseController<Employee, EmployeeDTO, UpdateEmployeeDTO>
{
    public EmployeeController(IEmployeeService service)
        : base(service) { }
}
