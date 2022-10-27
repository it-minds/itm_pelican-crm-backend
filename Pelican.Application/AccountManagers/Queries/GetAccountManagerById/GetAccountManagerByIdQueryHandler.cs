using Pelican.Application.Abstractions.Messaging;
using Pelican.Application.Common.Interfaces.DataLoaders;
using Pelican.Domain.Entities;

namespace Pelican.Application.AccountManagers.Queries.GetAccountManagerById;
public class GetAccountManagerByIdQueryHandler : IQueryHandler<GetAccountManagerByIdQuery, AccountManager>
{
	private readonly IGenericDataLoader<AccountManager> _dataLoader;
	public GetAccountManagerByIdQueryHandler(IGenericDataLoader<AccountManager> dataLoader)
	{
		_dataLoader = dataLoader;
	}
	//Uses dataloader to fetch a specific AccountManager in the database using their Id
	public async Task<AccountManager> Handle(GetAccountManagerByIdQuery request, CancellationToken cancellationToken)
	{
		return await _dataLoader.LoadAsync(request.Id, cancellationToken);
	}
}
