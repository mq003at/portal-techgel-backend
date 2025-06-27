using System.Text.Json;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using portal.Db;
using portal.DTOs;
using portal.Models;

namespace portal.Services;

public class NotificationCategoryService
    : BaseModelWithOnlyIdService<NotificationCategory, NotificationCategoryDTO, NotificationCategoryCreateDTO, NotificationCategoryUpdateDTO>,
      INotificationCategoryService
{
    public NotificationCategoryService(
        ApplicationDbContext context,
        IMapper mapper,
        ILogger<NotificationCategoryService> logger
    ) : base(context, mapper, logger)
    {
    }

    public override async Task<NotificationCategoryDTO> CreateAsync(NotificationCategoryCreateDTO dto)
    {
        var entity = _mapper.Map<NotificationCategory>(dto);

        // Gán mối liên hệ với các OrganizationEntity
        if (dto.OnlyForOrganizationEntityIds.Any())
        {
            var linkEntities = dto.OnlyForOrganizationEntityIds
                .Select(id => new OnlyForOrganizationEntity
                {
                    OrganizationEntityId = (int)id,
                    NotificationCategory = entity
                }).ToList();

            entity.OnlyForOrganizationEntities = linkEntities;
        }

        _dbSet.Add(entity);
        await _context.SaveChangesAsync();

        return _mapper.Map<NotificationCategoryDTO>(entity);
    }

    public override async Task<NotificationCategoryDTO?> UpdateAsync(int id, NotificationCategoryUpdateDTO dto)
    {
        var entity = await _dbSet
            .Include(nc => nc.OnlyForOrganizationEntities)
            .FirstOrDefaultAsync(nc => nc.Id == id);

        if (entity == null)
            return null;

        _mapper.Map(dto, entity);

        // Cập nhật quan hệ OnlyForOrganizationEntities nếu có
        if (dto.OnlyForOrganizationEntityIds != null)
        {
            entity.OnlyForOrganizationEntities.Clear();

            var newLinks = dto.OnlyForOrganizationEntityIds
                .Select(orgId => new OnlyForOrganizationEntity
                {
                    OrganizationEntityId = (int)orgId,
                    NotificationCategoryId = id
                }).ToList();

            entity.OnlyForOrganizationEntities.AddRange(newLinks);
        }

        await _context.SaveChangesAsync();

        return _mapper.Map<NotificationCategoryDTO>(entity);
    }
}
