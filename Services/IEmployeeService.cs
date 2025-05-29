namespace portal.Services;

using portal.DTOs;
using portal.Models;

public interface IEmployeeService
    : IBaseService<Employee, EmployeeDTO, CreateEmployeeDTO, UpdateEmployeeDTO>
{
    Task<EmployeeDTO> LoginAsync(string MainId, string password);
}
