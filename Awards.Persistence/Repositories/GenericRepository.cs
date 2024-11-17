using Awards.Application.Core.Abstractions.Data;
using Awards.Domain.Core.Primitives;
using Awards.Domain.Core.Primitives.Maybe;
using Awards.Persistence.Specifications;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Awards.Persistence.Repositories
{
    internal abstract class GenericRepository<TEntity>
        where TEntity : Entity
    {
        protected IDbContext DbContext { get; }

        protected GenericRepository(IDbContext dbContext) => DbContext = dbContext;
        
        public async Task<Maybe<TEntity>> GetByIdAsync(Guid id)
            => await DbContext.GetBydIdAsync<TEntity>(id);
        
        public void Insert(TEntity entity)
            => DbContext.Insert(entity);
        
        public void InsertRange(IReadOnlyCollection<TEntity> entities)
            => DbContext.InsertRange(entities);
        
        public void Update(TEntity entity)
            => DbContext.Set<TEntity>().Update(entity);

        public void Remove(TEntity entity)
            => DbContext.Remove(entity);

        protected async Task<bool> AnyAsync(Specification<TEntity> specification)
            => await DbContext.Set<TEntity>().AnyAsync(specification);

        protected async Task<Maybe<TEntity>> FirstOrDefaultAsync(Specification<TEntity> specification)
            => await DbContext.Set<TEntity>().FirstOrDefaultAsync(specification);
    }
}
