using MediatR;
using Pelican.Application.Security;
using Pelican.Application.Suppliers.Queries.GetSupplierById;
using Pelican.Application.Suppliers.Queries.GetSuppliers;
using Pelican.Domain.Entities;
using Pelican.Domain.Enums;

namespace Pelican.Presentation.GraphQL.Suppliers;

[Authorized(Role = RoleEnum.Standard)]
[Authorized(Role = RoleEnum.Admin)]
[ExtendObjectType("Query")]
public class SuppliersQuery
{
	//This Query reguests all Suppliers from the database.
	[UsePaging(IncludeTotalCount = true)]
	[UseProjection]
	[UseFiltering]
	[UseSorting]
	public async Task<IQueryable<Supplier>> GetSuppliers([Service] IMediator mediator, CancellationToken cancellationToken)
	{
		return await mediator.Send(new GetSuppliersQuery(), cancellationToken);
	}
	//This Query reguests a specific Supplier from the database.
	public async Task<Supplier> GetSupplierAsync(Guid id, [Service] IMediator mediator, CancellationToken cancellationToken)
	{
		var input = new GetSupplierByIdQuery(id);
		return await mediator.Send(input, cancellationToken);
	}
}
