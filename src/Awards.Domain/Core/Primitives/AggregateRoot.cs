using Awards.Domain.Core.Events;
using System;
using System.Collections.Generic;

namespace Awards.Domain.Core.Primitives
{
    public abstract class AggregateRoot : Entity
    {
        #region Read-Only Fields

        private readonly List<IDomainEvent> _domainEvents = new List<IDomainEvent>();

        #endregion

        #region Properties

        public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

        #endregion

        #region Constructors

        protected AggregateRoot(Guid id)
            : base(id)
        { }

        protected AggregateRoot()
        { }

        #endregion

        #region Methods

        public void ClearDomainEvents() => _domainEvents.Clear();

        protected void AddDomainEvent(IDomainEvent domainEvent) => _domainEvents.Add(domainEvent);

        #endregion
    }
}
