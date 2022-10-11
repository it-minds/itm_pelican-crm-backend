using HotChocolate.Types.Relay;
using MediatR;
using Pelican.Domain.Entities;

namespace Pelican.Application.Suppliers.Queries.GetSupplierById;
public record GetSupplierByIdQuery([ID(nameof(Supplier))] Guid Id) : IRequest<Supplier>;
