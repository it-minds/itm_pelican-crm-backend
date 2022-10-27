using HotChocolate.Types.Relay;
using Pelican.Application.Abstractions.Messaging;
using Pelican.Domain.Entities;

namespace Pelican.Application.AccountManagers.Queries.GetAccountManagerById;
public record GetAccountManagerByIdQuery([ID(nameof(AccountManager))] Guid Id) : IQuery<AccountManager>;
