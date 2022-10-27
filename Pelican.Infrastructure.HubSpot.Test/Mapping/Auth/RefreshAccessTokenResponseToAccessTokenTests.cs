using Pelican.Infrastructure.HubSpot.Contracts.Responses.Auth;
using Pelican.Infrastructure.HubSpot.Mapping.Auth;
using Xunit;

namespace Pelican.Infrastructure.HubSpot.Test.Mapping.Auth;

public class RefreshAccessTokenResponseToAccessTokenTests
{
	[Fact]
	public void ToString_ReturnCorrectProperties()
	{
		/// Arrange 
		const string ACCESSTOKEN = "accesstoken";

		RefreshAccessTokenResponse response = new()
		{
			AccessToken = ACCESSTOKEN,
		};

		/// Act
		var result = response.ToAccessToken();

		/// Assert
		Assert.Equal(ACCESSTOKEN, result);
	}
}
