namespace portal.Controllers;

using portal.DTOs;
using portal.Models;
using portal.Services;

public class SectionController : BaseController<Section, SectionDTO, UpdateSectionDTO>
{
    public SectionController(ISectionService SectionService)
        : base(SectionService) { }
}
