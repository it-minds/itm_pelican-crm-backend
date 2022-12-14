using Microsoft.AspNetCore.Http;
using Moq;
using Pelican.Presentation.Api.Utilities.HubSpotHookValidation.HashComputer;
using Pelican.Presentation.Api.Utilities.HubSpotHookValidation.HashGenerator;
using Xunit;

namespace Pelican.Presentation.Api.Test.Utilities.HubSpotHookValidation.HashGenerator;

public class V3HashGeneratorTests
{
	private const string CLIENTSECRET = "secret";
	private const string METHOD = "POST";
	private const string BODY = "{}";
	private const string PATH = "/path";

	private readonly Mock<IHashComputerFactory> _hashComputerFactoryMock;
	private readonly Mock<IHashComputer> _hashComputerMock;
	private readonly V3HashGenerator _uut;

	private readonly HttpContext _httpContext;

	public V3HashGeneratorTests()
	{
		_hashComputerFactoryMock = new();
		_hashComputerMock = new();

		_hashComputerFactoryMock
			.Setup(factory => factory.CreateClientSecretHashComputer(CLIENTSECRET))
			.Returns(_hashComputerMock.Object);

		_uut = new(
			CLIENTSECRET,
			_hashComputerFactoryMock.Object);

		_httpContext = new DefaultHttpContext();
	}

	[Fact]
	public void GenerateHash_NoTimestamp()
	{
		/// Arrange

		/// Act
		string result = _uut.GenerateHash(_httpContext.Request);

		/// Assert
		Assert.Equal(
			string.Empty,
			result);
	}

	[Fact]
	public void GenerateHash_InvalidTimestampFormat()
	{
		/// Arrange
		_httpContext.Request.Headers.Add(
			"X-HubSpot-Request-Timestamp",
			"Hello");

		/// Act
		string result = _uut.GenerateHash(_httpContext.Request);

		/// Assert
		Assert.Equal(
			string.Empty,
			result);
	}

	[Fact]
	public void GenerateHash_InvalidTimestamp()
	{
		/// Arrange
		long time = DateTimeOffset.Now.AddMinutes(-10).ToUnixTimeMilliseconds();

		_httpContext.Request.Headers.Add(
			"X-HubSpot-Request-Timestamp",
			time.ToString());

		/// Act
		string result = _uut.GenerateHash(_httpContext.Request);

		/// Assert
		Assert.Equal(
			string.Empty,
			result);
	}

	[Fact]
	public void GenerateHash_NoBody()
	{
		/// Arrange
		long time = DateTimeOffset.Now.AddMinutes(-1).ToUnixTimeMilliseconds();

		_httpContext.Request.Headers.Add(
			"X-HubSpot-Request-Timestamp",
			time.ToString());

		_httpContext.Request.ContentLength = 0;
		_httpContext.Request.Method = METHOD;
		_httpContext.Request.Path = PATH;

		string text = $"{METHOD}://{PATH}{time}";

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
		long time = DateTimeOffset.Now.AddMinutes(-1).ToUnixTimeMilliseconds();

		_httpContext.Request.Headers.Add(
			"X-HubSpot-Request-Timestamp",
			time.ToString());

		_httpContext.Request.ContentLength = 1;
		_httpContext.Request.Method = METHOD;
		_httpContext.Request.Path = PATH;

		MemoryStream stream = new();
		StreamWriter writer = new(stream);
		await writer.WriteAsync(BODY);
		await writer.FlushAsync();
		stream.Position = 0;

		_httpContext.Request.Body = stream;

		string text = $"{METHOD}://{PATH}{BODY}{time}";

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
