using MediatR;
using Pelican.Domain.Entities;

namespace Pelican.Application.Suppliers.Queries.GetSuppliers;
public record GetSuppliersQuery() : IRequest<IQueryable<Supplier>>;
