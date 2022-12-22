using HotChocolate.Types.Relay;
using Pelican.Application.Abstractions.Messaging;
using Pelican.Domain.Entities;

namespace Pelican.Application.Deals.Queries.GetDealById;
//TODO Re-add these lines when the login has been implemented
//[Authorize(Role = RoleEnum.Admin)]
//[Authorize(Role = RoleEnum.Standard)]
public record GetDealByIdQuery([ID(nameof(Deal))] Guid Id) : IQuery<Deal>;
