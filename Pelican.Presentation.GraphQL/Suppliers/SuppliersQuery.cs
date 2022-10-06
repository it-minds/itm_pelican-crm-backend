using HotChocolate.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Pelican.Domain.Entities;
using Pelican.Infrastructure.Persistence;

namespace Pelican.Presentation.GraphQL.Suppliers;
[ExtendObjectType("Query")]

public class SuppliersQuery
{
	[UseDbContext(typeof(IDbContext))]
	[UsePaging(IncludeTotalCount = true)]
	[UseProjection]
	[UseFiltering]
	[UseSorting]
	[Authorize]
	public IQueryable<Supplier> GetSuppliers([ScopedService] IDbContext context) => context.Suppliers.AsNoTracking();

	[Authorize]
	public Task<Supplier> GetSupplierAsync(Guid id, SupplierByIdDataLoader dataLoader, CancellationToken cancellationToken)
	{
		return dataLoader.LoadAsync(id, cancellationToken);
	}
}
