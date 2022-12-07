using Pelican.Domain;
using Pelican.Infrastructure.HubSpot.Contracts.Responses.Auth;
using Pelican.Infrastructure.HubSpot.Mapping.Auth;
using Xunit;

namespace Pelican.Infrastructure.HubSpot.Test.Mapping.Auth;

public class AccessTokenResponseToSupplierTests
{
	[Fact]
	public void ToSupplier_ReturnCorrectProperties()
	{
		/// Arrange 
		const string HUBDOMAIN = "hubdomain";
		const long HUBID = 123;

		AccessTokenResponse response = new()
		{
			HubDomain = HUBDOMAIN,
			HubId = HUBID,
		};

		/// Act
		var result = response.ToSupplier();

		/// Assert
		Assert.Equal(HUBDOMAIN, result.WebsiteUrl);
		Assert.Equal(HUBID, result.SourceId);
		Assert.Equal(Sources.HubSpot, result.Source);
	}
}
