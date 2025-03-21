namespace portal.Services;

using portal.DTOs;
using portal.Models;

public interface IDivisionService : IBaseService<Division, DivisionDTO, UpdateDivisionDTO>
{
    Task<IEnumerable<DivisionDTO>> GetFullHierarchyAsync();
}
