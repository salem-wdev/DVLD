using DVLD.Domain.Common;

namespace DVLD.Application.Interfaces.Persistence;

public interface IUnitOfWork : IDisposable
{
    // Tracks an entity so UnitOfWork knows which entities have pending domain events
    void TrackEntity(BaseEntity entity);

    Task BeginTransactionAsync();
    Task CommitAsync();
    Task RollbackAsync();
}