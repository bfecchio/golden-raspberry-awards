﻿using MediatR;

namespace Awards.Application.Core.Abstractions.Messaging
{
    public interface ICommand<out TResponse> : IRequest<TResponse>
    { }
}
