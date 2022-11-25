using Pelican.Application.Abstractions.Data.DataLoaders;
using Pelican.Application.Abstractions.Messaging;
using Pelican.Domain.Entities;

namespace Pelican.Application.Deals.Queries.GetDealById;
public class GetDealByIdQueryHandler : IQueryHandler<GetDealByIdQuery, Deal>
{
	private readonly IGenericDataLoader<Deal> _dataLoader;
	public GetDealByIdQueryHandler(IGenericDataLoader<Deal> dataLoader)
	{
		_dataLoader = dataLoader;
	}
	//Uses dataloader to fetch a specific Deal in the database using their Id
	public async Task<Deal> Handle(GetDealByIdQuery request, CancellationToken cancellationToken)
	{
		return await _dataLoader.LoadAsync(request.Id, cancellationToken);
	}
}
