using Pelican.Application.Abstractions.Messaging;
using Pelican.Domain.Entities;

namespace Pelican.Application.Suppliers.Queries.GetSuppliers;
//TODO Re-add these lines when the login has been implemented
//[Authorize(Role = RoleEnum.Admin)]
//[Authorize(Role = RoleEnum.Standard)]
public record GetSuppliersQuery() : IQuery<IQueryable<Supplier>>;
