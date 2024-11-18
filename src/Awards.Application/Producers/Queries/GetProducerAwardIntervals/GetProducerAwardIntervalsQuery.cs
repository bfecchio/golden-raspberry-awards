using Awards.Contracts.Producers;
using Awards.Domain.Core.Primitives.Maybe;
using Awards.Application.Core.Abstractions.Messaging;

namespace Awards.Application.Producers.Queries.GetProducerAwardIntervals
{
    public sealed class GetProducerAwardIntervalsQuery : IQuery<Maybe<ProducerAwardIntervalsResponse>>
    { }
}
