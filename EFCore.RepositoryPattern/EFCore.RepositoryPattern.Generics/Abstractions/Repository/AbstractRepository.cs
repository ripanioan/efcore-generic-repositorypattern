using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EFCore.RepositoryPattern.Generics.Abstractions.Repository
{
    public abstract class AbstractRepository<TEntity> : IAbstractRepository<TEntity>
        where TEntity : class
    {
        private readonly DbContext dbContext;

        public AbstractRepository(DbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public virtual IQueryable<TEntity> GetQueryable()
        {
            return dbContext.Set<TEntity>().AsNoTracking();
        }

        public virtual async Task<IList<TEntity>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await dbContext
                .Set<TEntity>()
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }

        public virtual async Task CreateAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            dbContext.Set<TEntity>().Add(entity);
            await SaveAsync(cancellationToken);
        }

        public virtual async Task CreateBulkAsync(ICollection<TEntity> entities, CancellationToken cancellationToken = default)
        {
            dbContext.Set<TEntity>().AddRange(entities);
            await SaveAsync(cancellationToken);
        }

        public virtual async Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            dbContext.Set<TEntity>().Update(entity);
            await SaveAsync(cancellationToken);
        }

        public virtual async Task UpdateBulkAsync(ICollection<TEntity> entities, CancellationToken cancellationToken = default)
        {
            dbContext.Set<TEntity>().UpdateRange(entities);
            await SaveAsync(cancellationToken);
        }

        public virtual async Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            dbContext.Set<TEntity>().Remove(entity);
            await SaveAsync(cancellationToken);
        }

        public virtual async Task DeleteBulkAsync(ICollection<TEntity> entities, CancellationToken cancellationToken = default)
        {
            dbContext.Set<TEntity>().RemoveRange(entities);
            await SaveAsync(cancellationToken);
        }

        public virtual async Task SaveAsync(CancellationToken cancellationToken = default)
        {
            _ = await dbContext.SaveChangesAsync(cancellationToken);
        }

        public virtual async Task<int> CountAsync(CancellationToken cancellationToken = default)
        {
            return await dbContext
                .Set<TEntity>()
                .AsNoTracking()
                .CountAsync(cancellationToken);
        }

        public virtual async Task<int> CountAsync(IQueryable<TEntity> query, CancellationToken cancellationToken = default)
        {
            return await query.CountAsync(cancellationToken);
        }
    }
}
