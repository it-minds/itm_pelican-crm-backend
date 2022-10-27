using Pelican.Domain.Entities;
using Pelican.Infrastructure.HubSpot.Contracts.Responses.Auth;

namespace Pelican.Infrastructure.HubSpot.Mapping.Auth;

internal static class AccessTokenResponseToSupplier
{
	internal static Supplier ToSupplier(this AccessTokenResponse response)
		=> new(Guid.NewGuid())
		{
			WebsiteUrl = response.HubDomain,
			HubSpotId = response.HubId,
		};
}
