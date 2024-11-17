using Awards.Api.Contracts;
using Awards.Api.Infrastructure;
using Awards.Application.Producers.Queries.GetProducerAwardIntervals;
using Awards.Contracts.Producers;
using Awards.Domain.Core.Primitives.Maybe;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Awards.Api.Controllers
{
    public sealed class ProducerController : ApiController
    {
        #region Constructors

        public ProducerController(IMediator mediator)
            : base(mediator)
        { }

        #endregion

        #region Controller Actions

        [HttpGet(ApiRoutes.Producers.GetProducerAwardIntervals)]
        [ProducesResponseType(typeof(ProducerAwardIntervalsResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetProducerAwardIntervals()
            => await Maybe<GetProducerAwardIntervalsQuery>
                .From(new GetProducerAwardIntervalsQuery())
                .Bind(query => Mediator.Send(query))
                .Match(Ok, NotFound);

        #endregion
    }
}
