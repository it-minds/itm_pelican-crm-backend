using HotChocolate.Types.Relay;
using MediatR;
using Pelican.Domain.Entities;

namespace Pelican.Application.AccountManagers.Queries.GetAccountManagerById;
public record GetAccountManagerByIdQuery([ID(nameof(AccountManager))] Guid Id) : IRequest<AccountManager>;
