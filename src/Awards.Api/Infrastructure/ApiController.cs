using Awards.Api.Contracts;
using Awards.Domain.Core.Primitives;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Awards.Api.Infrastructure
{
    [Route("api")]
    public class ApiController : ControllerBase
    {
        protected IMediator Mediator { get; }

        protected ApiController(IMediator mediator) => Mediator = mediator;
                
        protected new IActionResult Ok(object value) => base.Ok(value);
        protected new IActionResult NotFound() => base.NotFound();
        protected IActionResult BadRequest(Error error) => BadRequest(new ApiErrorResponse(new[] { error }));
    }
}
