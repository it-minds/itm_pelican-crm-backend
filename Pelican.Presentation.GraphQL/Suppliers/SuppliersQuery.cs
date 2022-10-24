using MediatR;
using Microsoft.EntityFrameworkCore;
using Pelican.Application.Suppliers.Queries.GetSupplierById;
using Pelican.Application.Suppliers.Queries.GetSuppliers;
using Pelican.Domain.Entities;

namespace Pelican.Presentation.GraphQL.Suppliers;
[ExtendObjectType("Query")]

public class SuppliersQuery
{
	//This Query reguests all Suppliers from the database.
	public async Task<IQueryable<Supplier>> GetSuppliers([Service] IMediator mediator, CancellationToken cancellationToken)
	{

		return (await mediator.Send(new GetSuppliersQuery(), cancellationToken))
			.Include(x => x.OfficeLocations)
			.Include(x => x.AccountManagers)
			.ThenInclude(x => x.AccountManagerDeals)
			.ThenInclude(x => x.Deal)
			.ThenInclude(x => x.Client)
			.Include(x => x.AccountManagers)
			.ThenInclude(x => x.AccountManagerDeals)
			.ThenInclude(x => x.Deal)
			.ThenInclude(x => x.DealContacts)
			.ThenInclude(x => x.Contact)
			.ThenInclude(x => x.ClientContacts)
			.ThenInclude(x => x.Client);
	}
	//This Query reguests a specific Supplier from the database.
	public async Task<Supplier> GetSupplierAsync(GetSupplierByIdQuery input, [Service] IMediator mediator, CancellationToken cancellationToken)
	{
		return await mediator.Send(input, cancellationToken);
	}
}
