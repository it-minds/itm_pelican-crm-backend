using Pelican.Application.Abstractions.Data.Repositories;
using Pelican.Domain.Entities;
using Pelican.Infrastructure.HubSpot.Contracts.Responses.Clients;

namespace Pelican.Infrastructure.HubSpot.Mapping.Clients;

internal static class CompaniesResponseToClients
{
	internal static async Task<List<Client>> ToClients(
		this CompaniesResponse responses,
		IUnitOfWork unitOfWork,
		CancellationToken cancellationToken)
	{
		if (responses.Results is null)
		{
			throw new ArgumentNullException(nameof(responses));
		}

		List<Client> results = new();

		foreach (CompanyResponse response in responses.Results)
		{
			Client result = await response.ToClient(unitOfWork, cancellationToken);
			results.Add(result);
		}

		return results;
	}
}
