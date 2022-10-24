using Microsoft.AspNetCore.Http;
using Moq;
using Pelican.Presentation.Api.Utilities.HubSpotHookValidation.HashComputer;
using Pelican.Presentation.Api.Utilities.HubSpotHookValidation.HashGenerator;
using Xunit;

namespace Pelican.Presentation.Api.Test.Utilities.HubSpotHookValidation.HashGenerator;

public class V1HashGeneratorTests
{
	private readonly string _clientSecret;
	private readonly Mock<IHashComputerFactory> _hashComputerFactoryMock;
	private readonly Mock<IHashComputer> _hashComputerMock;
	private readonly V1HashGenerator _uut;

	private readonly Mock<HttpRequest> _requestMock;

	public V1HashGeneratorTests()
	{
		_clientSecret = "secret";

		_hashComputerFactoryMock = new();
		_hashComputerMock = new();

		_hashComputerFactoryMock
			.Setup(factory => factory.CreateSHA256HashComputer())
			.Returns(_hashComputerMock.Object);

		_uut = new(
			_clientSecret,
			_hashComputerFactoryMock.Object);

		_requestMock = new();
	}

	[Fact]
	public void GenerateHash_NoBody()
	{
		/// Arrange
		_requestMock
			.Setup(request => request.ContentLength)
			.Returns(0);

		_hashComputerMock
			.Setup(hashComputer => hashComputer.ComputeHash(_clientSecret))
			.Returns("Computed Hash");

		/// Act
		string result = _uut.GenerateHash(_requestMock.Object);

		/// Assert
		_hashComputerMock
			.Verify(
				hashComputer => hashComputer.ComputeHash(_clientSecret),
				Times.Once);

		Assert.Equal(
			"Computed Hash",
			result);
	}
}

public class V2HashGeneratorTests
{
	private readonly string _clientSecret;
	private readonly Mock<IHashComputerFactory> _hashComputerFactoryMock;
	private readonly Mock<IHashComputer> _hashComputerMock;
	private readonly V1HashGenerator _uut;

	private readonly Mock<HttpRequest> _requestMock;

	public V2HashGeneratorTests()
	{
		_clientSecret = "secret";

		_hashComputerFactoryMock = new();
		_hashComputerMock = new();

		_hashComputerFactoryMock
			.Setup(factory => factory.CreateSHA256HashComputer())
			.Returns(_hashComputerMock.Object);

		_uut = new(
			_clientSecret,
			_hashComputerFactoryMock.Object);

		_requestMock = new();
	}

	[Fact]
	public void GenerateHash()
	{
		/// Arrange
		_requestMock
			.Setup(request => request.ContentLength)
			.Returns(0);

		_hashComputerMock
			.Setup(hashComputer => hashComputer.ComputeHash(_clientSecret))
			.Returns("Computed Hash");

		/// Act
		string result = _uut.GenerateHash(_requestMock.Object);

		/// Assert
		_hashComputerMock
			.Verify(
				hashComputer => hashComputer.ComputeHash(_clientSecret),
				Times.Once);

		Assert.Equal(
			"Computed Hash",
			result);
	}
}
