using Awards.Domain.Core.Primitives;
using Awards.Domain.Core.Primitives.Maybe;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Awards.Application.Core.Abstractions.Data
{
    public interface IDbContext
    {        
        DbSet<TEntity> Set<TEntity>()
            where TEntity : Entity;

        Task<Maybe<TEntity>> GetBydIdAsync<TEntity>(Guid id)
            where TEntity : Entity;

        void Insert<TEntity>(TEntity entity)
            where TEntity : Entity;

        void InsertRange<TEntity>(IReadOnlyCollection<TEntity> entities)
            where TEntity : Entity;

        void Remove<TEntity>(TEntity entity)
            where TEntity : Entity;        
    }
}
