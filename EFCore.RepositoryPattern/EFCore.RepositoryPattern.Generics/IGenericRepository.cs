using EFCore.RepositoryPattern.Generics.Abstractions.Domain;
using EFCore.RepositoryPattern.Generics.Abstractions.Repository;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace EFCore.RepositoryPattern.Generics
{
    public interface IGenericRepository<TEntity, TId> : IAbstractRepository<TEntity>
        where TEntity : class, IIdentifiable<TId>
        where TId : struct
    {
        Task<TEntity> GetByIdAsync(TId id, CancellationToken cancellationToken = default);
        Task<IList<TEntity>> GetByIdsAsync(IEnumerable<TId> ids, CancellationToken cancellationToken = default);

        Task UpdateAsync(TEntity entity, TId id, CancellationToken cancellationToken = default);

        Task DeleteAsync(TId id, CancellationToken cancellationToken = default);
        Task DeleteBulkAsync(IEnumerable<TId> ids, CancellationToken cancellationToken = default);
    }
}
