using Pelican.Application.Abstractions.Messaging;
using Pelican.Domain.Entities;

namespace Pelican.Application.Suppliers.Queries.GetSuppliers;
public record GetSuppliersQuery() : IQuery<IQueryable<Supplier>>;
