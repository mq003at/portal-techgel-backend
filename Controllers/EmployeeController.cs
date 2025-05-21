namespace portal.Controllers;

using portal.DTOs;
using portal.Models;
using portal.Services;

public class EmployeesController
    : BaseController<Employee, EmployeeDTO, CreateEmployeeDTO, UpdateEmployeeDTO>
{
    public EmployeesController(IEmployeeService service)
        : base(service) // service ở đây là IEmployeeService nhưng cũng hiện thực IBaseService<...>
    { }
}
