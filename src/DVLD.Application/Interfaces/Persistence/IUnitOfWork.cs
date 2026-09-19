using DVLD.Domain.Common;

namespace DVLD.Application.Interfaces.Persistence;

/// <summary>
/// Defines transactional boundary operations and domain entity tracking for persistence.
/// </summary>
public interface IUnitOfWork : IDisposable
{
    /// <summary>
    /// Tracks an entity instance to dispatch its recorded domain events during transaction commit.
    /// </summary>
    /// <param name="entity">The domain entity to track.</param>
    void TrackEntity(BaseEntity entity);

    /// <summary>
    /// Initiates a new transactional scope asynchronously.
    /// </summary>
    Task BeginTransactionAsync();

    /// <summary>
    /// Commits all pending transactional operations and triggers event dispatches.
    /// </summary>
    Task CommitAsync();

    /// <summary>
    /// Reverts all operations executed within the current active transaction scope.
    /// </summary>
    Task RollbackAsync();
}