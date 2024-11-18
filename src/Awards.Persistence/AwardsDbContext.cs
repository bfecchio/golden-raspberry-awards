using Awards.Application.Core.Abstractions.Data;
using Awards.Domain.Core.Primitives;
using Awards.Domain.Core.Primitives.Maybe;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Awards.Persistence
{
    public sealed class AwardsDbContext : DbContext, IDbContext, IUnitOfWork
    {
        private readonly IMediator _mediator;

        public AwardsDbContext(DbContextOptions options, IMediator mediator)
            : base(options)
        {
            _mediator = mediator;
        }

        public new DbSet<TEntity> Set<TEntity>()
            where TEntity : Entity
            => base.Set<TEntity>();

        public async Task<Maybe<TEntity>> GetBydIdAsync<TEntity>(Guid id)
            where TEntity : Entity
            => id == Guid.Empty
                ? Maybe<TEntity>.None
                : Maybe<TEntity>.From(await Set<TEntity>().FirstOrDefaultAsync(e => e.Id == id));

        public void Insert<TEntity>(TEntity entity)
            where TEntity : Entity
            => Set<TEntity>().Add(entity);

        public void InsertRange<TEntity>(IReadOnlyCollection<TEntity> entities)
            where TEntity : Entity
            => Set<TEntity>().AddRange(entities);

        public new void Remove<TEntity>(TEntity entity)
            where TEntity : Entity
            => Set<TEntity>().Remove(entity);

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {            
            await PublishDomainEvents(cancellationToken);
            return await base.SaveChangesAsync(cancellationToken);
        }

        public Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
            => Database.BeginTransactionAsync(cancellationToken);

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }

        private async Task PublishDomainEvents(CancellationToken cancellationToken)
        {
            var aggregateRoots = ChangeTracker
                .Entries<AggregateRoot>()
                .Where(entityEntry => entityEntry.Entity.DomainEvents.Any())
                .ToList();

            var domainEvents = aggregateRoots.SelectMany(entityEntry => entityEntry.Entity.DomainEvents).ToList();

            aggregateRoots.ForEach(entityEntry => entityEntry.Entity.ClearDomainEvents());

            var tasks = domainEvents.Select(domainEvent => _mediator.Publish(domainEvent, cancellationToken));

            await Task.WhenAll(tasks);
        }
    }
}
