namespace portal.Controllers;

using portal.DTOs;
using portal.Models;
using portal.Services;

public class UnitController : BaseController<Unit, UnitDTO, UpdateUnitDTO>
{
    public UnitController(IUnitService UnitService)
        : base(UnitService) { }
}
