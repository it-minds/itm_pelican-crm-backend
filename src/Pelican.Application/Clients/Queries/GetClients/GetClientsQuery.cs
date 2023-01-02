using Pelican.Application.Abstractions.Messaging;
using Pelican.Application.Security;
using Pelican.Domain.Entities;

namespace Pelican.Application.Clients.Queries.GetClients;
//TODO Re-add these lines when the login has been implemented
//[Authorize(Role = RoleEnum.Admin)]
//[Authorize(Role = RoleEnum.Standard)]
[Authenticated]
public record GetClientsQuery() : IQuery<IQueryable<Client>>;
