namespace portal.Services;

using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using portal.Db;
using portal.DTOs;
using portal.Models;

public class DivisionService
    : BaseService<Division, DivisionDTO, UpdateDivisionDTO>,
        IDivisionService
{
    protected new readonly ApplicationDbContext _context;
    protected new readonly IMapper _mapper;
    protected new readonly ILogger<DivisionService> _logger;

    public DivisionService(
        ApplicationDbContext context,
        IMapper mapper,
        ILogger<DivisionService> logger
    )
        : base(context, mapper, logger)
    {
        _context = context;
        _mapper = mapper;
        _logger = logger;
    }

    public override async Task<IEnumerable<DivisionDTO>> GetAllAsync()
    {
        var divisions = await _context.Divisions.Include(d => d.Departments).ToListAsync();

        return _mapper.Map<IEnumerable<DivisionDTO>>(divisions);
    }

    public override async Task<DivisionDTO?> GetByIdAsync(int id)
    {
        var division = await _context
            .Divisions.Include(d => d.Departments)
            .FirstOrDefaultAsync(d => d.Id == id);

        return division == null ? null : _mapper.Map<DivisionDTO>(division);
    }

    public async Task<IEnumerable<DivisionDTO>> GetFullHierarchyAsync()
    {
        var divisions = await _context.Divisions.ToListAsync();
        var departments = await _context.Departments.ToListAsync();
        var sections = await _context.Sections.ToListAsync();
        var units = await _context.Units.ToListAsync();
        var teams = await _context.Teams.ToListAsync();

        foreach (var unit in units)
            unit.Teams = teams.Where(t => t.UnitId == unit.Id).ToList();

        foreach (var section in sections)
            section.Units = units.Where(u => u.SectionId == section.Id).ToList();

        foreach (var department in departments)
            department.Sections = sections.Where(s => s.DepartmentId == department.Id).ToList();

        foreach (var division in divisions)
            division.Departments = departments.Where(d => d.DivisionId == division.Id).ToList();

        return _mapper.Map<IEnumerable<DivisionDTO>>(divisions);
    }
}
