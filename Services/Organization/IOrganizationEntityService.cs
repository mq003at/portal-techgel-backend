using portal.DTOs;
using portal.Models;

namespace portal.Services;

public interface IOrganizationEntityService
    : IBaseService<
        OrganizationEntity,
        OrganizationEntityDTO,
        CreateOrganizationEntityDTO,
        UpdateOrganizationEntityDTO>
{
    Task<bool> UpdateEmployeesAsync(int id, List<CreateOrganizationEntityEmployeeDTO> dto);
}