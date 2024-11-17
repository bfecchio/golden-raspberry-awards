using System.Threading;
using System.Threading.Tasks;
using Awards.Contracts.Producers;
using Awards.Domain.Core.Primitives.Maybe;
using Awards.Application.Core.Abstractions.Data;
using Awards.Application.Core.Abstractions.Messaging;

namespace Awards.Application.Producers.Queries.GetProducerAwardIntervals
{
    internal sealed class GetProducerAwardIntervalsQueryHandler : IQueryHandler<GetProducerAwardIntervalsQuery, Maybe<ProducerAwardIntervalsResponse>>
    {
        #region Read-Only Fields

        private readonly IDbContext _dbContext;

        #endregion

        public async Task<Maybe<ProducerAwardIntervalsResponse>> Handle(GetProducerAwardIntervalsQuery request, CancellationToken cancellationToken)
        {
            ProducerAwardIntervalsResponse response = null;

            return response;
        }
    }
}
