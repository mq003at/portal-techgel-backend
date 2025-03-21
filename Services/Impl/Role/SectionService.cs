namespace portal.Services;

using AutoMapper;
using portal.Db;
using portal.DTOs;
using portal.Models;

public class SectionService : BaseService<Section, SectionDTO, UpdateSectionDTO>, ISectionService
{
    public SectionService(
        ApplicationDbContext context,
        IMapper mapper,
        ILogger<SectionService> logger
    )
        : base(context, mapper, logger) { }
}
