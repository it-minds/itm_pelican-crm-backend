using Pelican.Domain.Entities;
using Pelican.Infrastructure.HubSpot.Contracts.Responses.AccountManagers;

namespace Pelican.Infrastructure.HubSpot.Mapping.AccountManagers;

internal static class OwnerResponseToAccountManager
{
	internal static AccountManager ToAccountManager(this OwnerResponse response) => new(Guid.NewGuid())
	{
		HubSpotId = response.Id,
		Email = response.Email,
		FirstName = response.Firstname,
		LastName = response.Lastname,
		HubSpotUserId = response.UserId,
	};
}
