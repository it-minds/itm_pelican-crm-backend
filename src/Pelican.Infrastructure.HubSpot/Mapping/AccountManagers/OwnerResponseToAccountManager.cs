using Pelican.Domain;
using Pelican.Domain.Entities;
using Pelican.Infrastructure.HubSpot.Contracts.Responses.AccountManagers;

namespace Pelican.Infrastructure.HubSpot.Mapping.AccountManagers;

internal static class OwnerResponseToAccountManager
{
	internal static AccountManager ToAccountManager(this OwnerResponse response)
	{
		if (
			string.IsNullOrWhiteSpace(response.Id)
			|| string.IsNullOrWhiteSpace(response.Email)
			|| string.IsNullOrWhiteSpace(response.Lastname)
			|| string.IsNullOrWhiteSpace(response.Firstname))
		{
			throw new ArgumentNullException(nameof(response));
		}
		AccountManager result = new(Guid.NewGuid())
		{
			SourceId = response.Id,
			SourceUserId = response.UserId,
			Email = response.Email,
			FirstName = response.Firstname,
			LastName = response.Lastname,
			Source = Sources.HubSpot,
		};
		return result;
	}
}
