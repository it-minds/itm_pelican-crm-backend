using Pelican.Infrastructure.HubSpot.Contracts.Responses.Auth;
using Pelican.Infrastructure.HubSpot.Mapping.Auth;
using Xunit;

namespace Pelican.Infrastructure.HubSpot.Test.Mapping.Auth;

public class GetAccessTokenResponseToRefreshAccessTokensTests
{
	[Fact]
	public void ToRefreshAccessTokens_ReturnCorrectProperties()
	{
		/// Arrange 
		const string ACCESSTOKEN = "accesstoken";
		const string REFRESHTOKEN = "refreshtoken";

		GetAccessTokenResponse response = new()
		{
			AccessToken = ACCESSTOKEN,
			RefreshToken = REFRESHTOKEN,
		};

		/// Act
		var result = response.ToRefreshAccessTokens();

		/// Assert
		Assert.Equal(ACCESSTOKEN, result.AccessToken);
		Assert.Equal(REFRESHTOKEN, result.RefreshToken);
	}
}
