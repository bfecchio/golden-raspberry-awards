using Awards.Domain.Core.Primitives;
using System.Collections.Generic;

namespace Awards.Api.Contracts
{
    public class ApiErrorResponse
    {        
        public ApiErrorResponse(IReadOnlyCollection<Error> errors) => Errors = errors;
 
        public IReadOnlyCollection<Error> Errors { get; }
    }
}
