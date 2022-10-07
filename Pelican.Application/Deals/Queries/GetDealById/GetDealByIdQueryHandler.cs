using MediatR;
using Pelican.Application.Common.Interfaces.DataLoaders;
using Pelican.Domain.Entities;

namespace Pelican.Application.Deals.Queries.GetDealById;
public class GetDealByIdQueryHandler : IRequestHandler<GetDealByIdQuery, Deal>
{
	private readonly IDealByIdDataLoader _dataLoader;
	public GetDealByIdQueryHandler(IDealByIdDataLoader dataLoader)
	{
		_dataLoader = dataLoader;
	}
	public async Task<Deal> Handle(GetDealByIdQuery request, CancellationToken cancellationToken)
	{
		return await _dataLoader.LoadAsync(request.Id, cancellationToken);
	}
}
