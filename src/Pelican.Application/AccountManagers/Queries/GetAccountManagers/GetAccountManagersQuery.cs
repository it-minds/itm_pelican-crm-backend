using Pelican.Application.Abstractions.Messaging;
using Pelican.Domain.Entities;

namespace Pelican.Application.AccountManagers.Queries.GetAccountManagers;
//TODO Re-add these lines when the login has been implemented
//[Authorize(Role = RoleEnum.Admin)]
//[Authorize(Role = RoleEnum.Standard)]
public record GetAccountManagersQuery() : IQuery<IQueryable<AccountManager>>;
