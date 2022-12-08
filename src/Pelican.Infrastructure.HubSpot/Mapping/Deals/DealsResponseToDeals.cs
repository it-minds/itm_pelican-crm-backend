using Pelican.Application.Abstractions.Data.Repositories;
using Pelican.Domain.Entities;
using Pelican.Infrastructure.HubSpot.Contracts.Responses.Deals;

namespace Pelican.Infrastructure.HubSpot.Mapping.Deals;

internal static class DealsResponseToDeals
{
	internal static async Task<List<Deal>> ToDeals(
		this DealsResponse responses,
		IUnitOfWork unitOfWork)
	{
		if (responses.Results is null)
		{
			throw new ArgumentNullException(nameof(responses));
		}

		List<Deal> results = new();

		foreach (DealResponse response in responses.Results)
		{
			Deal result = await response.ToDeal(unitOfWork);
			results.Add(result);
		}

		return results;
	}
}
