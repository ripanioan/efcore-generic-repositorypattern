using EFCore.RepositoryPattern.Generics.Abstractions.Domain;
using EFCore.RepositoryPattern.Generics.Abstractions.Repository;
using EFCore.RepositoryPattern.Generics.Exceptions;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EFCore.RepositoryPattern.Generics
{
    public abstract class GenericRepository<TEntity, TId> : AbstractRepository<TEntity>, IGenericRepository<TEntity, TId>
        where TEntity : class, IIdentifiable<TId>
        where TId : struct
    {
        private readonly DbContext dbContext;

        public GenericRepository(DbContext dbContext)
            : base(dbContext)
        {
            this.dbContext = dbContext;
        }

        public virtual async Task<TEntity> GetByIdAsync(TId id, CancellationToken cancellationToken = default)
        {
            var result = await dbContext
                .Set<TEntity>()
                .AsNoTracking()
                .FirstOrDefaultAsync(entity => id.Equals(entity.Id), cancellationToken);

            if (result is null)
            {
                throw new EntityNotFoundException<TEntity>();
            }

            return result;
        }

        public virtual async Task<IList<TEntity>> GetByIdsAsync(IEnumerable<TId> ids, CancellationToken cancellationToken = default)
        {
            var result = await dbContext
                .Set<TEntity>()
                .AsNoTracking()
                .Where(entity => ids.Contains(entity.Id))
                .Distinct()
                .ToListAsync(cancellationToken);

            if (!result.Any())
            {
                throw new EntityNotFoundException<TEntity>();
            }

            return result;
        }

        public async Task UpdateAsync(TEntity entity, TId id, CancellationToken cancellationToken = default)
        {
            _ = await GetByIdAsync(id, cancellationToken);

            await base.UpdateAsync(entity, cancellationToken);
        }

        public virtual async Task DeleteAsync(TId id, CancellationToken cancellationToken = default)
        {
            var entity = await GetByIdAsync(id, cancellationToken);

            await base.DeleteAsync(entity, cancellationToken);
        }

        public virtual async Task DeleteBulkAsync(IEnumerable<TId> ids, CancellationToken cancellationToken = default)
        {
            var entities = await GetByIdsAsync(ids, cancellationToken);

            await base.DeleteBulkAsync(entities, cancellationToken);
        }
    }
}
