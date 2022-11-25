using Pelican.Presentation.Api.Utilities.HubSpotHookValidation.HashComputer;
using Xunit;

namespace Pelican.Presentation.Api.Test.Utilities.HubSpotHookValidation.HashComputer;

public class HashComputerFactoryTests
{
	private const string CLIENTSECRET = "clientsecret";

	private readonly HashComputerFactory _uut;

	public HashComputerFactoryTests()
	{
		_uut = new();
	}

	[Fact]
	public void CreateSHA256HashComputer_ReturnsTypeSHA256HashComputer()
	{
		/// Act
		var result = _uut
			.CreateSHA256HashComputer();

		/// Assert
		Assert.Equal(
			typeof(SHA256HashComputer),
			result.GetType());
	}

	[Fact]
	public void CreateClientSecretHashComputer_ReturnsTypeClientSecretHashComputer()
	{
		/// Act
		var result = _uut
			.CreateClientSecretHashComputer(CLIENTSECRET);

		/// Assert
		Assert.Equal(
			typeof(ClientSecretHashComputer),
			result.GetType());
	}
}
