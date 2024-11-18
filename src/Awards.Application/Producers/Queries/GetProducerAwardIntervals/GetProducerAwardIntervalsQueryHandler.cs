using Awards.Application.Core.Abstractions.Data;
using Awards.Application.Core.Abstractions.Messaging;
using Awards.Contracts.Producers;
using Awards.Domain.Core.Primitives.Maybe;
using Awards.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Awards.Application.Producers.Queries.GetProducerAwardIntervals
{
    internal sealed class GetProducerAwardIntervalsQueryHandler : IQueryHandler<GetProducerAwardIntervalsQuery, Maybe<ProducerAwardIntervalsResponse>>
    {
        #region Read-Only Fields

        private readonly IDbContext _dbContext;

        #endregion

        #region Constructors

        public GetProducerAwardIntervalsQueryHandler(IDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        #endregion

        public async Task<Maybe<ProducerAwardIntervalsResponse>> Handle(GetProducerAwardIntervalsQuery request, CancellationToken cancellationToken)
        {
            // Filtra os filmes vencedores e organiza os dados necessários
            var winners = await _dbContext.Set<Movie>().AsNoTracking()
                .Where(m => m.Winner)
                .Select(m => new { m.Year, Producer = m.Producers })
                .OrderBy(m => m.Year) // Garante ordenação por ano para cálculo de intervalos
                .ToListAsync(cancellationToken);

            // Agrupa por produtor e calcula os intervalos de premiação
            var producerIntervals = winners
                .GroupBy(m => m.Producer)
                .Where(g => g.Count() > 1) // Considera apenas produtores com múltiplas vitórias
                .Select(g => g
                    .OrderBy(m => m.Year)
                    .Zip(g.OrderBy(m => m.Year).Skip(1), (previous, next) =>
                        new AwardIntervalResponse
                        {
                            Producer = g.Key,
                            Interval = next.Year - previous.Year,
                            PreviousWin = previous.Year,
                            FollowingWin = next.Year
                        })
                    )
                .SelectMany(intervals => intervals)
                .ToList();

            // Encontra o menor e maior intervalo
            var minInterval = producerIntervals
                .OrderBy(i => i.Interval)
                .GroupBy(i => i.Interval)
                .FirstOrDefault()?.ToList() ?? new List<AwardIntervalResponse>();

            var maxInterval = producerIntervals
                .OrderByDescending(i => i.Interval)
                .GroupBy(i => i.Interval)
                .FirstOrDefault()?.ToList() ?? new List<AwardIntervalResponse>();

            return new ProducerAwardIntervalsResponse
            {
                Min = minInterval,
                Max = maxInterval
            };
        }
    }
}
