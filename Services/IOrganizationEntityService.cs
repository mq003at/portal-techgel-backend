using portal.DTOs;
using portal.Models;

namespace portal.Services;

public interface IOrganizationEntityService
    : IBaseService<
        OrganizationEntity,
        OrganizationEntitySummaryDTO,
        CreateOrganizationEntityDTO,
        UpdateOrganizationEntityDTO
    > { }
