using Pelican.Domain.Entities;
using Pelican.Infrastructure.Pipedrive.Contracts.Responses.Deals;
using Pelican.Infrastructure.Pipedrive.Services;

namespace Pelican.Infrastructure.Pipedrive.Mapping.Deals;

internal static class DealResponseToDeal
{
	public static Deal ToDeal(DealResponse response)
	{
		return new Deal();
	}
}
