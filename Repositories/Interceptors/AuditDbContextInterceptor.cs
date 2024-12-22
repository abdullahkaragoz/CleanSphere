using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Repositories.Interceptors;

public class AuditDbContextInterceptor : SaveChangesInterceptor
{
    private static readonly Dictionary<EntityState, Action<DbContext, IAuditEntity>> Behaviors = new()
    {
        {EntityState.Added, AddBehavior},
        {EntityState.Modified, UpdateBehavior}
    };

    private static void AddBehavior(DbContext context, IAuditEntity auditEntity)
    {
        auditEntity.Created = DateTime.Now;
        context.Entry(auditEntity).Property(x => x.Updated).IsModified = false;

    }

    private static void UpdateBehavior(DbContext context, IAuditEntity auditEntity)
    {
        context.Entry(auditEntity).Property(x => x.Created).IsModified = false;
        auditEntity.Updated = DateTime.Now;
    }

    public override ValueTask<int> SavedChangesAsync(SaveChangesCompletedEventData eventData, int result, CancellationToken cancellationToken = default)
    {

        foreach (var entityEntry in eventData.Context!.ChangeTracker.Entries().ToList())
        {

            if (entityEntry.Entity is not IAuditEntity auditEntity) continue;


            if (entityEntry.State is not (EntityState.Added or EntityState.Modified)) continue;


            Behaviors[entityEntry.State](eventData.Context, auditEntity);

        }


        return base.SavedChangesAsync(eventData, result, cancellationToken);
    }

}
