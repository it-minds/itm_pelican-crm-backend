namespace Pelican.Infrastructure.HubSpot.Test.Services;

public class HubSpotClientServicesTests
{
	private readonly Mock<IClient<HubSpotSettings>> _hubSpotClientMock = new();
	private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
	private readonly HubSpotClientService _uut;

	public HubSpotClientServicesTests()
	{
		_uut = new HubSpotClientService(_hubSpotClientMock.Object, _unitOfWorkMock.Object);
	}

	[Fact]
	public void HubSpotClientService_ClientNull_ThrowException()
	{
		// Act
		var result = Record.Exception(() => new HubSpotClientService(null!, _unitOfWorkMock.Object));

		// Assert
		Assert.IsType<ArgumentNullException>(result);
		Assert.Contains(
			"client",
			result.Message);
	}
	[Fact]
	public void HubSpotClientService_UnitOfWorkNull_ThrowException()
	{
		// Act
		var result = Record.Exception(() => new HubSpotClientService(_hubSpotClientMock.Object, null!));

		// Assert
		Assert.IsType<ArgumentNullException>(result);
		Assert.Contains(
			"unitOfWork",
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
			.Setup(r => r.GetResultWithUnitOfWork(
				It.IsAny<Func<CompanyResponse, IUnitOfWork, CancellationToken, Task<Client>>>(),
				It.IsAny<IUnitOfWork>(),
				It.IsAny<CancellationToken>()))
			.ReturnsAsync(Result.Failure<Client>(Error.NullValue));

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

		Client client = new();

		_hubSpotClientMock
			.Setup(client => client.GetAsync<CompanyResponse>(
				It.IsAny<RestRequest>(),
				It.IsAny<CancellationToken>()))
			.ReturnsAsync(responseMock.Object);

		responseMock
			.Setup(r => r.GetResultWithUnitOfWork(
				It.IsAny<Func<CompanyResponse, IUnitOfWork, CancellationToken, Task<Client>>>(),
				It.IsAny<IUnitOfWork>(),
				It.IsAny<CancellationToken>()))
			.ReturnsAsync(client);

		/// Act
		var result = await _uut.GetByIdAsync("", 0, default);

		/// Assert
		Assert.True(result.IsSuccess);
		Assert.Equal(
			client,
			result.Value);
	}

	[Fact]
	public async Task GetAsync_ClientReturnsFailure_ReturnFailure()
	{
		/// Arrange
		Mock<IResponse<PaginatedResponse<CompanyResponse>>> responseMock = new();

		_hubSpotClientMock
			.Setup(client => client.GetAsync<PaginatedResponse<CompanyResponse>>(
				It.IsAny<RestRequest>(),
				It.IsAny<CancellationToken>()))
			.ReturnsAsync(responseMock.Object);

		responseMock
			.Setup(r => r.GetResultWithUnitOfWork(
				It.IsAny<Func<PaginatedResponse<CompanyResponse>, IUnitOfWork, CancellationToken, Task<List<Client>>>>(),
				It.IsAny<IUnitOfWork>(),
				It.IsAny<CancellationToken>()))
			.ReturnsAsync(Result.Failure<List<Client>>(Error.NullValue));

		/// Act
		var result = await _uut.GetAsync("", default);

		/// Assert
		Assert.True(result.IsFailure);
	}

	[Fact]
	public async Task GetAsync_ClientReturnsSuccess_ReturnSuccess()
	{
		/// Arrange
		Mock<IResponse<PaginatedResponse<CompanyResponse>>> responseMock = new();

		List<Client> Clients = new();

		_hubSpotClientMock
			.Setup(client => client.GetAsync<PaginatedResponse<CompanyResponse>>(
				It.IsAny<RestRequest>(),
				It.IsAny<CancellationToken>()))
			.ReturnsAsync(responseMock.Object);


		responseMock
			.Setup(r => r.GetResultWithUnitOfWork(
				It.IsAny<Func<PaginatedResponse<CompanyResponse>, IUnitOfWork, CancellationToken, Task<List<Client>>>>(),
				It.IsAny<IUnitOfWork>(),
				It.IsAny<CancellationToken>()))
			.ReturnsAsync(Result.Success(Clients));

		/// Act
		var result = await _uut.GetAsync("", default);

		/// Assert
		Assert.True(result.IsSuccess);
		Assert.Equal(
			Clients,
			result.Value);
	}

	[Fact]
	public async Task GetAsync_ClientReturnsSuccessTwice_ReturnSuccess()
	{
		/// Arrange
		Mock<IResponse<PaginatedResponse<CompanyResponse>>> responseMock0 = new();
		Mock<IResponse<PaginatedResponse<CompanyResponse>>> responseMock1 = new();

		List<Client> clients = new();

		Paging p = new()
		{
			Next = new()
			{
				After = "1",
			}
		};

		responseMock0
			.Setup(r => r.Data)
			.Returns(new PaginatedResponse<CompanyResponse>()
			{
				Paging = p,
			});

		_hubSpotClientMock
			.SetupSequence(client => client.GetAsync<PaginatedResponse<CompanyResponse>>(
				It.IsAny<RestRequest>(),
				It.IsAny<CancellationToken>()))
			.ReturnsAsync(responseMock0.Object)
			.ReturnsAsync(responseMock1.Object);

		responseMock0
			.Setup(r => r.GetResultWithUnitOfWork(
				It.IsAny<Func<PaginatedResponse<CompanyResponse>, IUnitOfWork, CancellationToken, Task<List<Client>>>>(),
				It.IsAny<IUnitOfWork>(),
				It.IsAny<CancellationToken>()))
			.ReturnsAsync(clients);

		responseMock1
			.Setup(r => r.GetResultWithUnitOfWork(
				It.IsAny<Func<PaginatedResponse<CompanyResponse>, IUnitOfWork, CancellationToken, Task<List<Client>>>>(),
				It.IsAny<IUnitOfWork>(),
				It.IsAny<CancellationToken>()))
			.ReturnsAsync(clients);

		/// Act
		var result = await _uut.GetAsync("", default);

		/// Assert
		Assert.True(result.IsSuccess);
		Assert.Equal(
			clients,
			result.Value);
	}
}
