using MediatR;
using Pelican.Application.Common.Interfaces.DataLoaders;
using Pelican.Domain.Entities;

namespace Pelican.Application.Suppliers.Queries.GetSupplierById;
public class GetSupplierByIdQueryHandler : IRequestHandler<GetSupplierByIdQuery, Supplier>
{
	private readonly ISupplierByIdDataLoader _dataLoader;
	public GetSupplierByIdQueryHandler(ISupplierByIdDataLoader dataLoader)
	{
		_dataLoader = dataLoader;
	}
	public async Task<Supplier> Handle(GetSupplierByIdQuery request, CancellationToken cancellationToken)
	{
		return await _dataLoader.LoadAsync(request.Id, cancellationToken);
	}
}
