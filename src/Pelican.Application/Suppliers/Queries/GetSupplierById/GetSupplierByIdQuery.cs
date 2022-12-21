using HotChocolate.Types.Relay;
using Pelican.Application.Abstractions.Messaging;
using Pelican.Application.Security;
using Pelican.Domain.Entities;
using Pelican.Domain.Enums;

namespace Pelican.Application.Suppliers.Queries.GetSupplierById;
[Authorize(Role = RoleEnum.Admin)]
[Authorize(Role = RoleEnum.Standard)]
public record GetSupplierByIdQuery([ID(nameof(Supplier))] Guid Id) : IQuery<Supplier>;
