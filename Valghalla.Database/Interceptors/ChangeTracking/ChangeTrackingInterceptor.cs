using Valghalla.Application.Exceptions;
using Valghalla.Application.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Valghalla.Database.Interceptors.ChangeTracking
{
    public sealed class ChangeTrackingInterceptor : SaveChangesInterceptor
    {
        private readonly IUserContextProvider userContextProvider;
        public ChangeTrackingInterceptor(IUserContextProvider userContextProvider)
        {
            this.userContextProvider = userContextProvider;
        }
        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
        {
            if (eventData.Context == null)
                return base.SavingChangesAsync(eventData, result, cancellationToken);

            HandleTracking(eventData);

            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
        {
            if (eventData.Context == null)
                return base.SavingChanges(eventData, result);

            HandleTracking(eventData);

            return base.SavingChanges(eventData, result);

        }

        private void HandleTracking(DbContextEventData eventData)
        {
            DbContext dbContext = eventData.Context!;

            IEnumerable<EntityEntry<IChangeTrackingEntity>> entries =
                dbContext
                    .ChangeTracker
                    .Entries<IChangeTrackingEntity>();

            if (!entries.Any()) return;

            Guid? currentUserId = userContextProvider.CurrentUser?.UserId;

            if (!currentUserId.HasValue && !currentUserId.Value.Equals(Guid.Empty))
            {
                throw new AnonymousUserException();
            }

            foreach (EntityEntry<IChangeTrackingEntity> entityEntry in entries)
            {
                switch (entityEntry.State)
                {
                    case EntityState.Modified:
                        entityEntry.Property(p => p.ChangedAt).CurrentValue = DateTime.UtcNow;
                        entityEntry.Property(p => p.ChangedBy).CurrentValue = currentUserId;
                        break;
                    case EntityState.Added:
                        entityEntry.Property(p => p.CreatedAt).CurrentValue = DateTime.UtcNow;
                        entityEntry.Property(p => p.CreatedBy).CurrentValue = currentUserId.Value;
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
