using Pelican.Application.Abstractions.Messaging;
using Pelican.Application.Security;
using Pelican.Domain.Entities;
using Pelican.Domain.Enums;

namespace Pelican.Application.Suppliers.Queries.GetSuppliers;
[Authorize(Role = RoleEnum.Admin)]
[Authorize(Role = RoleEnum.Standard)]
public record GetSuppliersQuery() : IQuery<IQueryable<Supplier>>;
