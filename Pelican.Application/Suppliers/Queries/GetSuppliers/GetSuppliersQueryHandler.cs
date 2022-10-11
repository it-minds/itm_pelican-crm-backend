using MediatR;
using Pelican.Domain.Entities;
using Pelican.Domain.Repositories;

namespace Pelican.Application.Suppliers.Queries.GetSuppliers;
public class GetSuppliersQueryHandler : IRequestHandler<GetSuppliersQuery, IQueryable<Supplier>>
{
	private readonly IGenericRepository<Supplier> _repository;
	public GetSuppliersQueryHandler(IUnitOfWork unitOfWork)
	{
		_repository = unitOfWork.SupplierRepository;
	}
	//Uses the repository for Supplier to find all Suppliers in the database
	public async Task<IQueryable<Supplier>> Handle(GetSuppliersQuery request, CancellationToken cancellation)
	{
		return _repository.FindAll();
	}
}
