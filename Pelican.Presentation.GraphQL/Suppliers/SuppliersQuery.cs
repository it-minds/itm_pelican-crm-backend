using MediatR;
using Pelican.Application.Suppliers.Queries.GetSupplierById;
using Pelican.Application.Suppliers.Queries.GetSuppliers;
using Pelican.Domain.Entities;

namespace Pelican.Presentation.GraphQL.Suppliers;
[ExtendObjectType("Query")]

public class SuppliersQuery
{
	public async Task<IQueryable<Supplier>> GetSuppliers([Service] IMediator mediator, CancellationToken cancellationToken)
	{
		return await mediator.Send(new GetSuppliersQuery(), cancellationToken);
	}

	public async Task<Supplier> GetSupplierAsync(GetSupplierByIdQuery input, [Service] IMediator mediator, CancellationToken cancellationToken)
	{
		return await mediator.Send(input, cancellationToken);
	}
}
