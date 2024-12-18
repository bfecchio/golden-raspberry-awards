﻿using Awards.Domain.Core.Primitives;
using System;

namespace Awards.Domain.Core.Exceptions
{
    public class DomainException : Exception
    {
        #region Properties

        public Error Error { get; }

        #endregion

        #region Constructors

        public DomainException(Error error)
            : base(error.Message)
            => Error = error;

        #endregion       
    }
}
