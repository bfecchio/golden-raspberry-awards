using Awards.Domain.Core.Primitives;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Awards.Application.Core.Exceptions
{
    public sealed class ValidationException : Exception
    {
        public IReadOnlyCollection<Error> Errors { get; }

        public ValidationException(IEnumerable<ValidationFailure> failures)
            : base("One or more validation failures has occurred.") =>
            Errors = failures
                .Distinct()
                .Select(failure => new Error(failure.ErrorCode, failure.ErrorMessage))
                .ToList();
    }
}
