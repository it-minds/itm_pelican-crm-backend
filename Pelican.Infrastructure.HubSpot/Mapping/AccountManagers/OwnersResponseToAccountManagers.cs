using Pelican.Domain.Entities;
using Pelican.Infrastructure.HubSpot.Contracts.Responses.AccountManagers;

namespace Pelican.Infrastructure.HubSpot.Mapping.AccountManagers;

internal static class OwnersResponseToAccountManagers
{
	internal static List<AccountManager> ToAccountManagers(this OwnersResponse responses)
	{
		List<AccountManager> results = new();

		foreach (OwnerResponse response in responses.Results)
		{
			AccountManager result = response.ToAccountManager();
			results.Add(result);
		}

		return results;
	}
}
