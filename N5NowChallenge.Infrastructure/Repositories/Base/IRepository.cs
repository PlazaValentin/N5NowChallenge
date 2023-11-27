using N5NowChallenge.Domain.Entities.Base;

namespace N5NowChallenge.Infrastructure.Repositories.Base;

public interface IGenericRepository { }

public interface IRepository<T> : IGenericRepository where T : Entity
{
    Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<T?> FindAsync(int id, CancellationToken cancellationToken = default);
    Task<T> CreateAsync(T entity, CancellationToken cancellationToken = default);
    void Update(T entity);
}