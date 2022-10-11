using Pelican.Domain.Entities;
using Pelican.Domain.Shared;

namespace Pelican.Application.Abstractions.HubSpot;

public interface IHubSpotContactService
{
	Task<Result<Supplier>> GetSupplierByIdAsync(string refreshToken, long id, CancellationToken cancellationToken);
}
