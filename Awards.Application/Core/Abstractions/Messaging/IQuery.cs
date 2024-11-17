using MediatR;

namespace Awards.Application.Core.Abstractions.Messaging
{
    public interface IQuery<out TResponse> : IRequest<TResponse>
    { }
}
