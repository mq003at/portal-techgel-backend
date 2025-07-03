namespace portal.Services;

using portal.DTOs;
using portal.Models;

public interface IEmployeeService
    : IBaseService<Employee, EmployeeDTO, CreateEmployeeDTO, UpdateEmployeeDTO>
{
    Task<IEnumerable<EmployeeDTO>> GetPhoneBookAllEmployeesAsync();
    Task<EmployeeDTO> LoginAsync(string MainId, string password);
    Task<List<string>> GetUserNamesByIdsAsync(List<int> userIds);
    Task<EmployeeDTO> UpdateEmployeeDetailsAsync(int employeeId, UpdateEmployeeDetailsDTO dto);
    Task<bool> ChangePasswordAsync(int employeeId, string oldPassword, string newPassword);
}
