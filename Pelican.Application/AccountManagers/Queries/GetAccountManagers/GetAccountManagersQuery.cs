using MediatR;
using Pelican.Domain.Entities;

namespace Pelican.Application.AccountManagers.Queries.GetAccountManagers;
public record GetAccountManagersQuery() : IRequest<IQueryable<AccountManager>>;
