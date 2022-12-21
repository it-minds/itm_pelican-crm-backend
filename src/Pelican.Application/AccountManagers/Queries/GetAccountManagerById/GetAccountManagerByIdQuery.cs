using HotChocolate.Types.Relay;
using Pelican.Application.Abstractions.Messaging;
using Pelican.Application.Security;
using Pelican.Domain.Entities;
using Pelican.Domain.Enums;

namespace Pelican.Application.AccountManagers.Queries.GetAccountManagerById;
[Authorize(Role = RoleEnum.Admin)]
[Authorize(Role = RoleEnum.Standard)]
public record GetAccountManagerByIdQuery([ID(nameof(AccountManager))] Guid Id) : IQuery<AccountManager>;
