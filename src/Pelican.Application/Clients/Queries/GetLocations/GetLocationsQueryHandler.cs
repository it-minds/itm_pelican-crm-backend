using Pelican.Application.Abstractions.Data.Repositories;
using Pelican.Application.Abstractions.Messaging;
using Pelican.Domain.Entities;

namespace Pelican.Application.Clients.Queries.GetLocations;

public class GetLocationsQueryHandler : IQueryHandler<GetLocationsQuery, IQueryable<Client>>
{
	private readonly IGenericRepository<Client> _repository;
	public GetLocationsQueryHandler(IUnitOfWork unitOfWork)
	{
		_repository = unitOfWork?.ClientRepository ?? throw new ArgumentNullException(nameof(unitOfWork));
	}

	public async Task<IQueryable<Client>> Handle(
		GetLocationsQuery request,
		CancellationToken cancellationToken)
		=> await Task.FromResult(_repository
			.FindByCondition(x => x.OfficeLocation != null)
			.Select(x => new Client() { OfficeLocation = x.OfficeLocation })
			.Distinct());
}
