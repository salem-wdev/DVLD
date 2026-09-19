using System.Data;
using DVLD.Application.Interfaces.Events;
using DVLD.Application.Interfaces.Persistence;
using DVLD.Domain.Common;
using DVLD.Infrastructure.Persistence.Context;

namespace DVLD.Infrastructure.Persistence;

/// <summary>
/// Coordinates persistence operations and transactional boundaries,
/// orchestrating pre-commit and post-commit domain event dispatches.
/// </summary>
public sealed class UnitOfWork : IUnitOfWork
{
    private readonly DbSession _session;
    private readonly IEventAggregator _eventAggregator;
    private readonly List<BaseEntity> _trackedEntities = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="UnitOfWork"/> class.
    /// </summary>
    /// <param name="session">The current database session holding transaction context.</param>
    /// <param name="eventAggregator">The event aggregator responsible for publishing domain events.</param>
    public UnitOfWork(DbSession session, IEventAggregator eventAggregator)
    {
        _session = session;
        _eventAggregator = eventAggregator;
    }

    /// <summary>
    /// Registers an entity to have its recorded domain events dispatched during commit.
    /// </summary>
    /// <param name="entity">The entity containing domain events to track.</param>
    public void TrackEntity(BaseEntity entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        if (!_trackedEntities.Contains(entity))
        {
            _trackedEntities.Add(entity);
        }
    }

    /// <summary>
    /// Begins a new transaction scope if none is active.
    /// </summary>
    public async Task BeginTransactionAsync()
    {
        if (_session.Connection.State != ConnectionState.Open)
        {
            await _session.Connection.OpenAsync();
        }

        _session.Transaction = _session.Connection.BeginTransaction();
    }

    /// <summary>
    /// Commits the active transaction, executing before-commit handlers in-transaction,
    /// and dispatching post-commit side-effect handlers upon persistence success.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown when attempting to commit without an active transaction.</exception>
    public async Task CommitAsync()
    {
        if (_session.Transaction is null)
        {
            throw new InvalidOperationException("No active database transaction is available to commit.");
        }

        try
        {
            // 1. Aggregate all pending domain events from tracked entities
            var events = _trackedEntities
                .SelectMany(entity => entity.DomainEvents)
                .ToList();

            // 2. Execute before-commit handlers within the active transaction scope
            if (events.Count > 0)
            {
                await _eventAggregator.PublishBeforeCommitAsync(events);
            }

            // 3. Atomically commit the transaction to persistent storage
            await _session.Transaction.CommitAsync();

            // 4. Clear domain events from tracked entities to prevent duplicate dispatching
            foreach (var entity in _trackedEntities)
            {
                entity.ClearDomainEvents();
            }
            _trackedEntities.Clear();

            // 5. Execute post-commit handlers for side-effects (e.g., messaging, notifications)
            if (events.Count > 0)
            {
                await _eventAggregator.PublishAfterCommitAsync(events);
            }
        }
        catch
        {
            // Rollback all database operations if any step fails
            await RollbackAsync();
            throw;
        }
        finally
        {
            // Always ensure the transaction instance is disposed and cleared
            _session.Transaction.Dispose();
            _session.Transaction = null;
        }
    }

    /// <summary>
    /// Rolls back the active transaction and clears all tracked state.
    /// </summary>
    public async Task RollbackAsync()
    {
        if (_session.Transaction is not null)
        {
            await _session.Transaction.RollbackAsync();
            _session.Transaction.Dispose();
            _session.Transaction = null;
        }

        _trackedEntities.Clear();
    }

    /// <summary>
    /// Releases allocated transaction and tracking resources.
    /// </summary>
    public void Dispose()
    {
        _session.Transaction?.Dispose();
        _session.Transaction = null;
        _trackedEntities.Clear();
    }
}