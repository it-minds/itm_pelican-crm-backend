namespace Pelican.Infrastructure.HubSpot.Test.Services;

public class HubSpotClientServicesTests
{
	private readonly Mock<IClient<HubSpotSettings>> _hubSpotClientMock;
	private readonly HubSpotClientService _uut;

	public HubSpotClientServicesTests()
	{
		_hubSpotClientMock = new();

		_uut = new HubSpotClientService(_hubSpotClientMock.Object);
	}

	[Fact]
	public void HubSpotClientService_ClientNull_ThrowException()
	{
		// Act
		var result = Record.Exception(() => new HubSpotClientService(null!));

		// Assert
		Assert.IsType<ArgumentNullException>(result);
		Assert.Contains(
			"client",
			result.Message);
	}

	[Fact]
	public async Task GetByIdAsync_ClientReturnsFailure_ReturnFailure()
	{
		/// Arrange
		Mock<IResponse<CompanyResponse>> responseMock = new();

		_hubSpotClientMock
			.Setup(client => client.GetAsync<CompanyResponse>(
				It.IsAny<RestRequest>(),
				It.IsAny<CancellationToken>()))
			.ReturnsAsync(responseMock.Object);

		responseMock
			.Setup(r => r.GetResult(It.IsAny<Func<CompanyResponse, Client>>()))
			.Returns(Result.Failure<Client>(Error.NullValue));

		/// Act
		var result = await _uut.GetByIdAsync("", 0, default);

		/// Assert
		Assert.True(result.IsFailure);
	}

	[Fact]
	public async Task GetByIdAsync_ClientReturnsSuccess_ReturnSuccess()
	{
		/// Arrange
		Mock<IResponse<CompanyResponse>> responseMock = new();

		Client Client = new();

		_hubSpotClientMock
			.Setup(client => client.GetAsync<CompanyResponse>(
				It.IsAny<RestRequest>(),
				It.IsAny<CancellationToken>()))
			.ReturnsAsync(responseMock.Object);

		responseMock
			.Setup(r => r.GetResult(It.IsAny<Func<CompanyResponse, Client>>()))
			.Returns(Client);

		/// Act
		var result = await _uut.GetByIdAsync("", 0, default);

		/// Assert
		Assert.True(result.IsSuccess);
		Assert.Equal("id", result.Value.SourceId);
	}

	[Fact]
	public async Task GetAsync_ClientReturnsNull_ReturnFailure()
	{
		/// Arrange
		RestResponse<CompaniesResponse> restResponse = null!;

		_hubSpotClientMock
			.Setup(client => client.GetAsync<CompaniesResponse>(
				It.IsAny<RestRequest>(),
				default))
			.ReturnsAsync(restResponse);

		/// Act
		var result = await _uut.GetAsync("", default);

		/// Assert
		Assert.True(result.IsFailure);
	}

	[Fact]
	public async Task GetAsync_ClientReturnsFailure_ReturnFailure()
	{
		/// Arrange
		Mock<IResponse<CompaniesResponse>> responseMock = new();

		_hubSpotClientMock
			.Setup(client => client.GetAsync<CompaniesResponse>(
				It.IsAny<RestRequest>(),
				It.IsAny<CancellationToken>()))
			.ReturnsAsync(responseMock.Object);

		responseMock
			.Setup(r => r.GetResult(It.IsAny<Func<CompaniesResponse, List<Client>>>()))
			.Returns(Result.Failure<List<Client>>(Error.NullValue));

		/// Act
		var result = await _uut.GetAsync("", default);

		/// Assert
		Assert.True(result.IsFailure);
	}

	[Fact]
	public async Task GetAsync_ClientReturnsSuccess_ReturnSuccess()
	{
		/// Arrange
		Mock<IResponse<CompaniesResponse>> responseMock = new();

		List<Client> Clients = new();

		_hubSpotClientMock
			.Setup(client => client.GetAsync<CompaniesResponse>(
				It.IsAny<RestRequest>(),
				It.IsAny<CancellationToken>()))
			.ReturnsAsync(responseMock.Object);

		responseMock
			.Setup(r => r.GetResult(It.IsAny<Func<CompaniesResponse, List<Client>>>()))
			.Returns(Clients);

		/// Act
		var result = await _uut.GetAsync("", default);

		/// Assert
		Assert.True(result.IsSuccess);
		Assert.Equal(
			Clients,
			result.Value);
	}
}
