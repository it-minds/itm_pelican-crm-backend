using Pelican.Domain.Entities;
using Pelican.Infrastructure.HubSpot.Contracts.Responses.AccountManagers;
using Pelican.Infrastructure.HubSpot.Contracts.Responses.Common;

namespace Pelican.Infrastructure.HubSpot.Mapping.AccountManagers;

internal static class OwnersResponseToAccountManagers
{
	internal static List<AccountManager> ToAccountManagers(this PaginatedResponse<OwnerResponse> responses)
	{
		if (responses.Results is null)
		{
			throw new ArgumentNullException(nameof(responses));
		}

		List<AccountManager> results = new();

		foreach (OwnerResponse response in responses.Results)
		{
			AccountManager result = response.ToAccountManager();
			results.Add(result);
		}

		return results;
	}
}
