using Pelican.Infrastructure.HubSpot.Contracts.Responses.Auth;
using Xunit;

namespace Pelican.Infrastructure.HubSpot.Test.Mapping.Auth;

public class RefreshAccessTokenResponseToStringTests
{
	[Fact]
	public void ToString_ReturnCorrectProperties()
	{
		/// Arrange 
		const string ACCESSTOKEN = "accesstoken";

		GetAccessTokenResponse response = new()
		{
			AccessToken = ACCESSTOKEN,
		};

		/// Act
		var result = response.ToString();

		/// Assert
		Assert.Equal(ACCESSTOKEN, result);
	}
}
