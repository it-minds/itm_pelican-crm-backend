using MediatR;
using Pelican.Application.Common.Interfaces.DataLoaders;
using Pelican.Domain.Entities;

namespace Pelican.Application.Suppliers.Queries.GetSupplierById;
public class GetSupplierByIdQueryHandler : IRequestHandler<GetSupplierByIdQuery, Supplier>
{
	private readonly IGenericDataLoader<Supplier> _dataLoader;
	public GetSupplierByIdQueryHandler(IGenericDataLoader<Supplier> dataLoader)
	{
		_dataLoader = dataLoader;
	}
	//Uses dataloader to fetch a specific Supplier in the database using their Id 
	public async Task<Supplier> Handle(GetSupplierByIdQuery request, CancellationToken cancellationToken)
	{
		return await _dataLoader.LoadAsync(request.Id, cancellationToken);
	}
}
