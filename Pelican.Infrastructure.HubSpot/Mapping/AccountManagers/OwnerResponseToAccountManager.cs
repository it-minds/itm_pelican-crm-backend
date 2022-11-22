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

		return new(Guid.NewGuid())
		{
			HubSpotId = response.Id,
			Email = response.Email.Length > StringLengths.Email ? response.Email.Substring(0, StringLengths.Email - 3) + ("...") : response.Email,
			FirstName = response.Firstname.Length > StringLengths.Name ? response.Firstname.Substring(0, StringLengths.Name - 3) + ("...") : response.Firstname,
			LastName = response.Lastname.Length > StringLengths.Name ? response.Lastname.Substring(0, StringLengths.Name - 3) + ("...") : response.Lastname,
			HubSpotUserId = response.UserId,
		};
	}
}
