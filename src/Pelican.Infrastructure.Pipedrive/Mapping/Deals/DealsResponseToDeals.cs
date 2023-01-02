using Pelican.Domain.Entities;
using Pelican.Infrastructure.Pipedrive.Contracts.Responses.Deals;
using Pelican.Infrastructure.Pipedrive.Services;

namespace Pelican.Infrastructure.Pipedrive.Mapping.Deals;

internal static class DealsResponseToDeals
{
	public static List<Deal> ToDeals(DealsResponse response)
	{
		// PLCN.233
		return new List<Deal>();
	}
}
