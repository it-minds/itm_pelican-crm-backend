using Microsoft.AspNetCore.Http;
using Moq;
using Pelican.Presentation.Api.Utilities.HubSpotHookValidation.HashComputer;
using Pelican.Presentation.Api.Utilities.HubSpotHookValidation.HashGenerator;
using Xunit;

namespace Pelican.Presentation.Api.Test.Utilities.HubSpotHookValidation.HashGenerator;

public class V2HashGeneratorTests
{
	private const string CLIENTSECRET = "secret";
	private const string METHOD = "POST";
	private const string BODY = "{}";
	private const string PATH = "/path";

	private readonly Mock<IHashComputerFactory> _hashComputerFactoryMock;
	private readonly Mock<IHashComputer> _hashComputerMock;
	private readonly V2HashGenerator _uut;

	private readonly HttpContext _httpContext;

	public V2HashGeneratorTests()
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
		_httpContext.Request.Method = METHOD;
		_httpContext.Request.Path = PATH;

		string text = $"{CLIENTSECRET}{METHOD}://{PATH}";

		_hashComputerMock
			.Setup(hashComputer => hashComputer.ComputeHash(text))
			.Returns("Computed Hash");

		/// Act
		string result = _uut.GenerateHash(_httpContext.Request);

		/// Assert
		_hashComputerMock
			.Verify(
				hashComputer => hashComputer.ComputeHash(text),
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
		_httpContext.Request.Method = METHOD;
		_httpContext.Request.Path = PATH;

		MemoryStream stream = new();
		StreamWriter writer = new(stream);
		await writer.WriteAsync(BODY);
		await writer.FlushAsync();
		stream.Position = 0;

		_httpContext.Request.Body = stream;

		string text = $"{CLIENTSECRET}{METHOD}://{PATH}{BODY}";

		_hashComputerMock
			.Setup(hashComputer => hashComputer.ComputeHash(text))
			.Returns("Computed Hash");

		/// Act
		string result = _uut.GenerateHash(_httpContext.Request);

		/// Assert
		_hashComputerMock
			.Verify(
				hashComputer => hashComputer.ComputeHash(text),
				Times.Once);

		Assert.Equal(
			"Computed Hash",
			result);
	}
}
