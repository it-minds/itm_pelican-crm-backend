using Microsoft.AspNetCore.Http;
using Moq;
using Pelican.Presentation.Api.Utilities.HubSpotHookValidation.HashComputer;
using Pelican.Presentation.Api.Utilities.HubSpotHookValidation.HashGenerator;
using Xunit;

namespace Pelican.Presentation.Api.Test.Utilities.HubSpotHookValidation.HashGenerator;

public class V1HashGeneratorTests
{
	private const string CLIENTSECRET = "secret";
	private const string BODY = "{}";

	private readonly Mock<IHashComputerFactory> _hashComputerFactoryMock;
	private readonly Mock<IHashComputer> _hashComputerMock;
	private readonly V1HashGenerator _uut;

	private readonly HttpContext _httpContext;

	public V1HashGeneratorTests()
	{
		_hashComputerFactoryMock = new();
		_hashComputerMock = new();

		_hashComputerFactoryMock
			.Setup(factory => factory.CreateSHA256HashComputer())
			.Returns(_hashComputerMock.Object);

		_uut = new(
			CLIENTSECRET,
			_hashComputerFactoryMock.Object);

		_httpContext = new DefaultHttpContext();
	}

	[Fact]
	public void GenerateHash_NoBody()
	{
		/// Arrange
		_httpContext.Request.ContentLength = 0;

		_hashComputerMock
			.Setup(hashComputer => hashComputer.ComputeHash(CLIENTSECRET))
			.Returns("Computed Hash");

		/// Act
		string result = _uut.GenerateHash(_httpContext.Request);

		/// Assert
		_hashComputerMock
			.Verify(
				hashComputer => hashComputer.ComputeHash(CLIENTSECRET),
				Times.Once);

		Assert.Equal(
			"Computed Hash",
			result);
	}

	[Fact]
	public async Task GenerateHash_WithBody()
	{
		/// Arrange
		_httpContext.Request.ContentLength = 1;

		MemoryStream stream = new();
		StreamWriter writer = new(stream);
		await writer.WriteAsync(BODY);
		await writer.FlushAsync();
		stream.Position = 0;

		_httpContext.Request.Body = stream;

		_hashComputerMock
			.Setup(hashComputer => hashComputer.ComputeHash($"{CLIENTSECRET}{BODY}"))
			.Returns("Computed Hash");

		/// Act
		string result = _uut.GenerateHash(_httpContext.Request);

		/// Assert
		_hashComputerMock
			.Verify(
				hashComputer => hashComputer.ComputeHash($"{CLIENTSECRET}{BODY}"),
				Times.Once);

		Assert.Equal(
			"Computed Hash",
			result);
	}
}
