using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.EntityFrameworkCore;
using Wafi.Abp.Workspace.Core;
using Wafi.Abp.Workspace.Services;

namespace Wafi.Abp.Workspace
{
    public abstract class WorkspaceDbContextBase<TSelf>
    : AbpDbContext<TSelf>
    where TSelf : DbContext
    {
        protected WorkspaceDbContextBase(DbContextOptions<TSelf> options)
        : base(options)
        {
        }

        protected ICurrentWorkspace CurrentWorkspace =>
        LazyServiceProvider.LazyGetRequiredService<ICurrentWorkspace>();

        protected bool IsMultiWorkspaceFilterEnabled =>
            DataFilter?.IsEnabled<IWorkspace>() ?? false;

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            ApplyCurrentWorkspaceId();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(
            bool acceptAllChangesOnSuccess,
            CancellationToken cancellationToken = default)
        {
            ApplyCurrentWorkspaceId();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        private void ApplyCurrentWorkspaceId()
        {
            if (CurrentWorkspace?.Id == null) return;

            var currentWorkspaceId = CurrentWorkspace.Id.Value;

            foreach (var entry in ChangeTracker.Entries()
                .Where(e =>
                    e.Entity is IWorkspace &&
                    (e.State == EntityState.Added || e.State == EntityState.Modified)))
            {
                // Stamp the FK column via EF Core API
                entry.Property(nameof(IWorkspace.WorkspaceId)).CurrentValue = currentWorkspaceId;

                if (entry.State == EntityState.Modified)
                {
                    // Prevent accidental overwrites
                    entry.Property(nameof(IWorkspace.WorkspaceId)).IsModified = false;
                }
            }
        }

        protected override bool ShouldFilterEntity<TEntity>(IMutableEntityType entityType)
        {
            if (typeof(IWorkspace).IsAssignableFrom(typeof(TEntity)))
            {
                return true;
            }

            return base.ShouldFilterEntity<TEntity>(entityType);
        }

        protected override Expression<Func<TEntity, bool>> CreateFilterExpression<TEntity>(ModelBuilder modelBuilder)
        {
            var baseExpression = base.CreateFilterExpression<TEntity>(modelBuilder);

            if (!typeof(IWorkspace).IsAssignableFrom(typeof(TEntity)))
            {
                return baseExpression;
            }

            var prop = modelBuilder
            .Entity<TEntity>()
                .Metadata
                .FindProperty(nameof(IWorkspace.WorkspaceId))!;

            var columnName = prop.GetColumnName() ?? prop.Name;

            Expression<Func<TEntity, bool>> hotelFilter = e =>
                !IsMultiWorkspaceFilterEnabled
                || CurrentWorkspace.Id == null
                || EF.Property<Guid?>(e, columnName) == CurrentWorkspace.Id;

            if (baseExpression == null) return hotelFilter;

            return QueryFilterExpressionHelper.CombineExpressions(baseExpression, hotelFilter);
        }
    }
}
