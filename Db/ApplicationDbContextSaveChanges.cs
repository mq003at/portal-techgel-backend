namespace portal.Db;

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using portal.Models;

public class AppDbContextSaveChangesInterceptor : SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default
    )
    {
        var context = eventData.Context;
        if (context == null)
            return base.SavingChangesAsync(eventData, result, cancellationToken);

        var utcNow = DateTime.UtcNow;

        foreach (var entry in context.ChangeTracker.Entries())
        {
            if (entry.Entity is BaseModel baseModel)
            {
                if (entry.State == EntityState.Added)
                {
                    baseModel.CreatedAt = utcNow;
                    baseModel.UpdatedAt = utcNow;
                }
                else if (entry.State == EntityState.Modified)
                {
                    baseModel.UpdatedAt = utcNow;
                    entry.Property(nameof(baseModel.CreatedAt)).IsModified = false; // Don't overwrite CreatedAt
                }
            }
        }

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}
