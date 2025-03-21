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
    public override InterceptionResult<int> SavingChanges(
        DbContextEventData eventData,
        InterceptionResult<int> result
    )
    {
        ConvertDateTimeFieldsToUtc(eventData);
        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default
    )
    {
        ConvertDateTimeFieldsToUtc(eventData);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private void ConvertDateTimeFieldsToUtc(DbContextEventData eventData)
    {
        if (eventData.Context == null)
            return;

        var entries = eventData
            .Context.ChangeTracker.Entries()
            .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

        foreach (var entry in entries)
        {
            foreach (var property in entry.Entity.GetType().GetProperties())
            {
                if (
                    property.PropertyType == typeof(DateTime)
                    || property.PropertyType == typeof(DateTime?)
                )
                {
                    var value = property.GetValue(entry.Entity);
                    if (value is DateTime dateTimeValue && dateTimeValue.Kind != DateTimeKind.Utc)
                    {
                        property.SetValue(
                            entry.Entity,
                            DateTime.SpecifyKind(dateTimeValue, DateTimeKind.Utc)
                        );
                    }
                }
            }
        }
    }
}
