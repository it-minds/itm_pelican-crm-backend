namespace Pelican.Infrastructure.HubSpot.Mapping.Auth;

internal static class AccessTokenResponseToSupplier
{
	internal static Supplier ToSupplier(this AccessTokenResponse response)
	{
		var domainStrings = response.HubDomain.Split('.');
		return new()
		{
			WebsiteUrl = response.HubDomain,
			SourceId = response.HubId,
			Source = Sources.HubSpot,
			Name = domainStrings[0] == "www"
				? domainStrings[1]
				: domainStrings[0],
		};
	}
}
