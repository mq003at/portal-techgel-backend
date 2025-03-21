namespace portal.Services;

using Microsoft.EntityFrameworkCore;
using portal.Db;
using portal.DTOs;
using portal.Models;

public class HierarchyService
{
    private readonly ApplicationDbContext _context;

    public HierarchyService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Department>> GetDepartmentsByDivision(int divisionId)
    {
        return await _context.Departments.Where(d => d.DivisionId == divisionId).ToListAsync();
    }

    public async Task<List<Section>> GetSectionsByDepartment(int departmentId)
    {
        return await _context.Sections.Where(s => s.DepartmentId == departmentId).ToListAsync();
    }

    public async Task<List<Unit>> GetUnitsBySection(int sectionId)
    {
        return await _context.Units.Where(u => u.SectionId == sectionId).ToListAsync();
    }

    public async Task<List<Team>> GetTeamsByUnit(int unitId)
    {
        return await _context.Teams.Where(t => t.UnitId == unitId).ToListAsync();
    }
}
