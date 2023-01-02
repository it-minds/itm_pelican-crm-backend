using Pelican.Application.Abstractions.Data.Repositories;
using Pelican.Application.RestSharp;
using Pelican.Domain.Entities;
using Pelican.Infrastructure.HubSpot.Contracts.Responses.Common;
using Pelican.Infrastructure.HubSpot.Contracts.Responses.Deals;

namespace Pelican.Infrastructure.HubSpot.Mapping.Deals;

internal static class DealsResponseToDeals
{
	internal static async Task<List<Deal>> ToDeals(
		this PaginatedResponse<DealResponse> responses,
		IUnitOfWork unitOfWork,
		CancellationToken cancellationToken)
	{
		if (responses.Results is null)
		{
			throw new ArgumentNullException(nameof(responses));
		}

		List<Deal> results = new();

		foreach (DealResponse response in responses.Results)
		{
			Deal result = await response.ToDeal(unitOfWork, cancellationToken);
			results.Add(result);
		}

		return results;
	}
}
