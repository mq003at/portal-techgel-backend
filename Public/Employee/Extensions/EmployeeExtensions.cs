using portal.Models;

namespace portal.Extensions;

public static class EmployeeExtensions
{
    public static string GetDisplayName(this Employee employee)
    {
        return $"{employee.LastName} {employee.MiddleName} {employee.FirstName}";
    }
}