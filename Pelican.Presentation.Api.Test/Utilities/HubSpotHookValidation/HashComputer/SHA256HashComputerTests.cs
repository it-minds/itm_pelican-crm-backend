using Pelican.Presentation.Api.Utilities.HubSpotHookValidation.HashComputer;
using Xunit;

namespace Pelican.Presentation.Api.Test.Utilities.HubSpotHookValidation.HashComputer;

public class SHA256HashComputerTests
{
	private readonly SHA256HashComputer _uut;

	public SHA256HashComputerTests()
	{
		_uut = new();
	}

	[Fact]
	public void ComputeHash()
	{
		/// Arrange
		/// HubSpot Example
		string clientSecret = "yyyyyyyy-yyyy-yyyy-yyyy-yyyyyyyyyyyy";
		string body = "[{" +
			"\"eventId\":1," +
			"\"subscriptionId\":12345," +
			"\"portalId\":62515," +
			"\"occurredAt\":1564113600000," +
			"\"subscriptionType\":\"contact.creation\"," +
			"\"attemptNumber\":0," +
			"\"objectId\":123," +
			"\"changeSource\":\"CRM\"," +
			"\"changeFlag\":\"NEW\"," +
			"\"appId\":54321}]";

		string text = clientSecret + body;

		/// Act
		string result = _uut.ComputeHash(text);

		/// Assert
		Assert.Equal(
			"232db2615f3d666fe21a8ec971ac7b5402d33b9a925784df3ca654d05f4817de",
			result);
	}
}
