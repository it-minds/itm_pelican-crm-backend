using HotChocolate.Types.Relay;
using Pelican.Application.Abstractions.Messaging;
using Pelican.Domain.Entities;

namespace Pelican.Application.AccountManagers.Queries.GetAccountManagerById;
//TODO Re-add these lines when the login has been implemented
//[Authorize(Role = RoleEnum.Admin)]
//[Authorize(Role = RoleEnum.Standard)]
public record GetAccountManagerByIdQuery([ID(nameof(AccountManager))] Guid Id) : IQuery<AccountManager>;
