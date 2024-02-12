using DataAccess.Base.Interfaces;

using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;

using TeamsMaker.Api.DataAccess.Base.Interfaces;

namespace TeamsMaker.Api.DataAccess.Interceptors;

public class EntitySaveChangesInterceptor : SaveChangesInterceptor
{
    private readonly IServiceProvider _serviceProvider;
    private readonly DateTime _now;

    public EntitySaveChangesInterceptor(IServiceProvider serviceProvider)
    {
        _now = DateTime.UtcNow;
        _serviceProvider = serviceProvider;
    }

    private IUserInfo _userInfo => _serviceProvider.GetRequiredService<IUserInfo>();


    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        _UpdateEntity(eventData.Context);
        return base.SavingChanges(eventData, result);
    }


    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        _UpdateEntity(eventData.Context);

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private void _UpdateEntity(DbContext? context)
    {
        if (context is null) return;

        foreach (var entityEntry in context.ChangeTracker.Entries().Where(e => e.State == EntityState.Added || e.State == EntityState.Modified))
        {
            if (entityEntry.State == EntityState.Added)
            {
                AddOrganizationInfo(entityEntry);
                AddUserInfo(entityEntry);
                AddCreationInfo(entityEntry);
                AddStatusInfo(entityEntry);

                continue;
            }

            // Update
            FreezeOrganizationInfo(entityEntry);
            AddModificationInfo(entityEntry);
            UpdateModificationInfo(entityEntry);
        }
    }

    private void FreezeOrganizationInfo(EntityEntry entityEntry)
    {
        if (entityEntry.Entity is not IOrganizationInfo || entityEntry.Entity is not IReadOnlyOrganizationInfo)
            return;

        var readonlyOrganizationEntity = entityEntry.Entity as IReadOnlyOrganizationInfo;
        entityEntry.Property(nameof(readonlyOrganizationEntity.OrganizationId)).IsModified = false;
    }

    private void UpdateModificationInfo(EntityEntry entityEntry)
    {
        if (entityEntry.Entity.GetType().GetInterfaces().Where(i => i.IsGenericType).Any(i => i.GetGenericTypeDefinition() == typeof(ILastStatus<>)))
        {
            var statusEntity = entityEntry.Entity as ILastStatus<IStatus>;
            if (!entityEntry.Property(nameof(statusEntity.LastStatusId)).IsModified) return;

            entityEntry.Property(nameof(statusEntity.LastStatusCreatedBy)).CurrentValue = _userInfo.UserName;
            entityEntry.Property(nameof(statusEntity.LastStatusDateTime)).CurrentValue = _now;
        }
    }

    private void AddModificationInfo(EntityEntry entityEntry)
    {
        if (entityEntry.Entity is IModificationInfo modificationInfoEntity)
        {
            entityEntry.Property(nameof(modificationInfoEntity.ModifiedBy)).CurrentValue = _userInfo.UserName;
            entityEntry.Property(nameof(modificationInfoEntity.LastModificationDate)).CurrentValue = _now;
        }
    }

    private void AddStatusInfo(EntityEntry entityEntry)
    {
        if (entityEntry.Entity.GetType().GetInterfaces().Where(i => i.IsGenericType).Any(i => i.GetGenericTypeDefinition() == typeof(ILastStatus<>)))
        {
            var statusEntity = entityEntry.Entity as ILastStatus<IStatus>;
            entityEntry.Property(nameof(statusEntity.LastStatusCreatedBy)).CurrentValue = _userInfo.UserName;
            entityEntry.Property(nameof(statusEntity.LastStatusDateTime)).CurrentValue = _now;
        }
    }

    private void AddCreationInfo(EntityEntry entityEntry)
    {
        if (entityEntry.Entity is not ICreationInfo creationInfoEntity) return;

        entityEntry.Property(nameof(creationInfoEntity.CreatedBy)).CurrentValue = _userInfo.UserName;
        entityEntry.Property(nameof(creationInfoEntity.CreationDate)).CurrentValue = _now;
    }

    private void AddUserInfo(EntityEntry entityEntry)
    {
        if (entityEntry.Entity is IUserRelatedEntity userRelatedEntity)
            entityEntry.Property(nameof(userRelatedEntity.UserId)).CurrentValue = _userInfo.UserId;
    }

    private void AddOrganizationInfo(EntityEntry entityEntry)
    {
        if (entityEntry.Entity is IOrganizationInfo organizationEntity)
            organizationEntity.OrganizationId = _userInfo.OrganizationId;
        else if (entityEntry.Entity is IReadOnlyOrganizationInfo readOnlyOrganizationEntity)
            entityEntry.Property(nameof(readOnlyOrganizationEntity.OrganizationId)).CurrentValue = _userInfo.OrganizationId;
    }
}
