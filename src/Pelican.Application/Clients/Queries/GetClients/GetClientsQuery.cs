using Pelican.Application.Abstractions.Messaging;
using Pelican.Application.Security;
using Pelican.Domain.Entities;
using Pelican.Domain.Enums;

namespace Pelican.Application.Clients.Queries.GetClients;
//TODO Re-add these lines when the login has been implemented
[Authorize(Role = RoleEnum.Admin)]
[Authorize(Role = RoleEnum.Standard)]
public record GetClientsQuery() : IQuery<IQueryable<Client>>;
