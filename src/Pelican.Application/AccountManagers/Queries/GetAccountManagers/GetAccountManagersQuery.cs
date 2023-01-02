using Pelican.Application.Abstractions.Messaging;
using Pelican.Domain.Entities;

namespace Pelican.Application.AccountManagers.Queries.GetAccountManagers;
public record GetAccountManagersQuery() : IQuery<IQueryable<AccountManager>>;
