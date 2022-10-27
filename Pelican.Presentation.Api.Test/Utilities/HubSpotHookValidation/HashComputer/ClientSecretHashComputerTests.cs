using Pelican.Presentation.Api.Utilities.HubSpotHookValidation.HashComputer;
using Xunit;

namespace Pelican.Presentation.Api.Test.Utilities.HubSpotHookValidation.HashComputer;

public class ClientSecretHashComputerTests
{
	private readonly ClientSecretHashComputer _uut;

	public ClientSecretHashComputerTests()
	{
		string clientSecret = "HelloWorld";
		_uut = new(clientSecret);
	}

	[Fact]
	public void Ctor_MissingArg_ThrowException()
	{
		/// Act
		var result = Record.Exception(() => new ClientSecretHashComputer(null!));

		/// Assert
		Assert.NotNull(result);
	}

	[Fact]
	public void Ctor_NoMissingArg_NoException()
	{
		/// Act
		var result = Record.Exception(() => new ClientSecretHashComputer("clientSecret"));

		/// Assert
		Assert.Null(result);
	}



	[Fact]
	public void ComputeHash()
	{
		/// Arrange
		string text = "TestingText";

		/// Act
		string result = _uut.ComputeHash(text);

		/// Assert
		/// According to https://www.devglan.com/online-tools/hmac-sha256-online
		Assert.Equal(
			"7I8gFHm4xSCeN1rajhUgqq1Ue6HDHHmtZOZi/0pUC4Y=",
			result);
	}
}
