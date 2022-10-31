using Pelican.Domain.Entities;
using Pelican.Infrastructure.HubSpot.Contracts.Responses.Deals;

namespace Pelican.Infrastructure.HubSpot.Mapping.Deals;

internal static class DealsResponseToDeals
{
	internal static List<Deal> ToDeals(this DealsResponse responses)
	{
		if (responses.Results is null)
		{
			throw new ArgumentNullException(nameof(responses));
		}

		List<Deal> results = new();

		foreach (DealResponse response in responses.Results)
		{
			Deal result = response.ToDeal();
			results.Add(result);
		}

		return results;
	}
}
