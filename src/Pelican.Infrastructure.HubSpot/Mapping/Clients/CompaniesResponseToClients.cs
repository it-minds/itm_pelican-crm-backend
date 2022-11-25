using Pelican.Domain.Entities;
using Pelican.Infrastructure.HubSpot.Contracts.Responses.Clients;

namespace Pelican.Infrastructure.HubSpot.Mapping.Clients;

internal static class CompaniesResponseToClients
{
	internal static List<Client> ToClients(this CompaniesResponse responses)
	{
		if (responses.Results is null)
		{
			throw new ArgumentNullException(nameof(responses));
		}

		List<Client> results = new();

		foreach (CompanyResponse response in responses.Results)
		{
			Client result = response.ToClient();
			results.Add(result);
		}

		return results;
	}
}
