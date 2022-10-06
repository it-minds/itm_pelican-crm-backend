﻿using MediatR;
using Pelican.Application.Common.Interfaces.DataLoaders;
using Pelican.Domain.Entities;

namespace Pelican.Application.AccountManagers.Queries.GetAccountManagerById;
public class GetAccountManagerByIdQueryHandler : IRequestHandler<GetAccountManagerByIdQuery, AccountManager>
{
	private readonly IAccountManagerByIdDataLoader _dataLoader;
	public GetAccountManagerByIdQueryHandler(IAccountManagerByIdDataLoader dataLoader)
	{
		_dataLoader = dataLoader;
	}
	public async Task<AccountManager> Handle(GetAccountManagerByIdQuery request, CancellationToken cancellationToken)
	{
		return await _dataLoader.LoadAsync(request.Id, cancellationToken);
	}
}
