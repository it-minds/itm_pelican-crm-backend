using MediatR;
using Pelican.Domain.Shared;

namespace Pelican.Application.Abstractions.Messaging;
public interface ICommand : IRequest<Result>
{
}

public interface ICommand<TResponse> : IRequest<Result<TResponse>>
{
}
