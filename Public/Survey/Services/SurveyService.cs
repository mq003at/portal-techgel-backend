using System.Text.Json;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using portal.Db;
using portal.DTOs;
using portal.Models;

namespace portal.Services;

public class SurveyService
    : BaseService<Survey, SurveyDTO, SurveyCreateDTO, SurveyUpdateDTO>,
        ISurveyService
{
    public SurveyService(
        ApplicationDbContext context,
        IMapper mapper,
        ILogger<SurveyService> logger
    )
        : base(context, mapper, logger) { }

    public override async Task<SurveyDTO> CreateAsync(SurveyCreateDTO dto)
    {
        _logger.LogInformation($"Creating Survey: {JsonSerializer.Serialize(dto)}");

        // Map survey
        var entity = _mapper.Map<Survey>(dto);

        // Load recipients manually
        entity.Recipients = await _context
            .Employees.Where(e => dto.RecipientIds.Contains(e.Id))
            .ToListAsync();

        // Map questions
        entity.Questions = _mapper.Map<List<SurveyQuestion>>(dto.Questions);

        _dbSet.Add(entity);
        await _context.SaveChangesAsync();

        return _mapper.Map<SurveyDTO>(entity);
    }

    public override async Task<SurveyDTO?> UpdateAsync(int id, SurveyUpdateDTO dto)
    {
        var entity = await _dbSet
            .Include(s => s.Questions)
            .Include(s => s.Recipients)
            .FirstOrDefaultAsync(s => s.Id == id);

        if (entity == null)
            return null;

        if (entity.Status != SurveyStatus.DRAFT)
            throw new InvalidOperationException("Only surveys in DRAFT status can be updated.");

        _logger.LogInformation($"Updating Survey before: {JsonSerializer.Serialize(entity)}");

        // Update basic fields
        _mapper.Map(dto, entity);

        // Update recipients
        entity.Recipients = await _context
            .Employees.Where(e => dto.RecipientIds.Contains(e.Id))
            .ToListAsync();

        // Replace existing questions
        _context.SurveyQuestions.RemoveRange(entity.Questions);
        entity.Questions = _mapper.Map<List<SurveyQuestion>>(dto.Questions);

        await _context.SaveChangesAsync();

        var result = _mapper.Map<SurveyDTO>(entity);
        _logger.LogInformation($"Updated Survey after: {JsonSerializer.Serialize(result)}");

        return result;
    }

    public override async Task<bool> DeleteAsync(int id)
    {
        var entity = await _dbSet.FindAsync(id);
        if (entity == null)
            return false;

        if (entity.Status != SurveyStatus.DRAFT)
            throw new InvalidOperationException("Only surveys in DRAFT status can be deleted.");

        _dbSet.Remove(entity);
        await _context.SaveChangesAsync();
        return true;
    }
}
