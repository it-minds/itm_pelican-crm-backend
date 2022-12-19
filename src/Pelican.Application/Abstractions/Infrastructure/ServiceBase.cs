using Pelican.Application.Abstractions.Data.Repositories;

namespace Pelican.Application.Abstractions.Infrastructure;

public abstract class ServiceBase<TSettings>
{
	protected readonly IClient<TSettings> _client;
	protected readonly IUnitOfWork _unitOfWork;

	protected ServiceBase(
		IClient<TSettings> client,
		IUnitOfWork unitOfWork)
	{
		_client = client ?? throw new ArgumentNullException(nameof(client));
		_unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
	}
}
