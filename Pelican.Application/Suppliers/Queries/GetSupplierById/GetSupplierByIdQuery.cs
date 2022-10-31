using HotChocolate.Types.Relay;
using Pelican.Application.Abstractions.Messaging;
using Pelican.Domain.Entities;

namespace Pelican.Application.Suppliers.Queries.GetSupplierById;
public record GetSupplierByIdQuery([ID(nameof(Supplier))] Guid Id) : IQuery<Supplier>;
