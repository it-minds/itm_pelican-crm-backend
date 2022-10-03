using HotChocolate.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Pelican.Domain.Entities;
using Pelican.Infrastructure.Persistence;

namespace Pelican.Presentation.GraphQL.Suppliers;
[ExtendObjectType("Query")]

public class SuppliersQuery
{
	[UseDbContext(typeof(PelicanContext))]
	[UsePaging(IncludeTotalCount = true)]
	[UseProjection]
	[UseFiltering]
	[UseSorting]
	[Authorize]
	public IQueryable<Supplier> GetSuppliers([ScopedService] PelicanContext context) => context.Suppliers.AsNoTracking();

	[Authorize]
	public Task<Supplier> GetSupplierAsync(Guid id, SupplierByIdDataLoader dataLoader, CancellationToken cancellationToken)
	{
		return dataLoader.LoadAsync(id, cancellationToken);
	}
}
