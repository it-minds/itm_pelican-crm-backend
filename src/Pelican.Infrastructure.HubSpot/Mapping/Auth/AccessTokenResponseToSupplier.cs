using Pelican.Domain;
using Pelican.Domain.Entities;
using Pelican.Infrastructure.HubSpot.Contracts.Responses.Auth;

namespace Pelican.Infrastructure.HubSpot.Mapping.Auth;

internal static class AccessTokenResponseToSupplier
{
	internal static Supplier ToSupplier(this AccessTokenResponse response)
		=> new()
		{
			WebsiteUrl = response.HubDomain,
			SourceId = response.HubId,
			Source = Sources.HubSpot,
			Name = response.HubDomain.Split('.').First(),
		};
}
