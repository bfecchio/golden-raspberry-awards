using Awards.Domain.Core.Primitives;
using System;
using System.Linq.Expressions;

namespace Awards.Persistence.Specifications
{
    internal abstract class Specification<TEntity>
        where TEntity : Entity
    {
        internal abstract Expression<Func<TEntity, bool>> ToExpression();

        internal bool IsSatisfiedBy(TEntity entity) => ToExpression().Compile()(entity);

        public static implicit operator Expression<Func<TEntity, bool>>(Specification<TEntity> specification) =>
            specification.ToExpression();
    }
}
