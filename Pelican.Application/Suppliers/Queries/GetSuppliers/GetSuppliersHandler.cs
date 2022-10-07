using MediatR;
using Pelican.Domain.Entities;
using Pelican.Domain.Repositories;

namespace Pelican.Application.Suppliers.Queries.GetSuppliers;
public class GetSuppliersHandler : IRequestHandler<GetSuppliersQuery, IQueryable<Supplier>>
{
	private readonly ISupplierRepository _repository;
	public GetSuppliersHandler(ISupplierRepository supplierRepository)
	{
		_repository = supplierRepository;
	}
	public async Task<IQueryable<Supplier>> Handle(GetSuppliersQuery request, CancellationToken cancellation)
	{
		return _repository.FindAll();
	}
}
