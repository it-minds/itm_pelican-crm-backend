using MediatR;

namespace Pelican.Application.Abstractions.Messaging;
public interface IQuery<out TResponse> : IRequest<TResponse>
{
}
