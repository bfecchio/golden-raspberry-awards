using Awards.Application.Core.Abstractions.Data;
using Awards.Application.Core.Abstractions.Messaging;
using Awards.Contracts.Producers;
using Awards.Domain.Core.Primitives.Maybe;
using Awards.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
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
            var winners = (await _dbContext.Set<Movie>().AsNoTracking()
                .Where(a => a.Winner)
                .ToListAsync(cancellationToken))
                .Select(a => new { a.Year, Producers = Regex.Split(a.Producers, @", | and ").Select(p => p.Trim()) });

            // Agrupa por produtor e calcula os intervalos de premiação
            var producerWins = winners
                .SelectMany(a => a.Producers.Select(p => new { Producer = p, a.Year }))
                .GroupBy(p => p.Producer)
                .Where(g => g.Count() > 1) // Considera apenas produtores com múltiplas vitórias
                .Select(g => new
                {
                    Producer = g.Key,
                    Intervals = g
                        .OrderBy(a => a.Year)
                        .Zip(g.OrderBy(a => a.Year).Skip(1), (prev, next) =>
                            new AwardIntervalResponse
                            {
                                Producer = g.Key,
                                Interval = next.Year - prev.Year,
                                PreviousWin = prev.Year,
                                FollowingWin = next.Year
                            })
                        .ToList()
                })
                .ToList();

            // Encontra o menor e maior intervalo
            var minInterval = producerWins
                .SelectMany(p => p.Intervals)
                .OrderBy(i => i.Interval)
                .GroupBy(i => i.Interval)
                .FirstOrDefault()?.ToList() ?? new List<AwardIntervalResponse>();

            var maxInterval = producerWins
                .SelectMany(p => p.Intervals)
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
