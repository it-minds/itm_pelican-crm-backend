using Moq;
using Pelican.Infrastructure.HubSpot.Abstractions;
using Pelican.Infrastructure.HubSpot.Contracts.Responses.Clients;
using Pelican.Infrastructure.HubSpot.Services;
using RestSharp;
using Xunit;

namespace Pelican.Infrastructure.HubSpot.Test.Services;

public class HubSpotClientServicesTests
{
	private const string ID = "Id";

	private readonly Mock<IHubSpotClient> _hubSpotClientMock;
	private readonly HubSpotClientService _uut;
	private readonly CancellationToken _cancellationToken;

	public HubSpotClientServicesTests()
	{
		_hubSpotClientMock = new();
		_cancellationToken = new();
		_uut = new HubSpotClientService(_hubSpotClientMock.Object);
	}

	[Fact]
	public async Task GetByIdAsync_ClientReturnsFailure_ReturnFailure()
	{
		/// Arrange
		_hubSpotClientMock
			.Setup(client => client.GetAsync<CompanyResponse>(
				It.IsAny<RestRequest>(),
				_cancellationToken))
			.ReturnsAsync(new RestResponse<CompanyResponse>()
			{
				IsSuccessStatusCode = false,
			});

		/// Act
		var result = await _uut.GetByIdAsync("", 0, _cancellationToken);

		/// Assert
		Assert.True(result.IsFailure);
	}

	[Fact]
	public async Task GetByIdAsync_ClientReturnsSuccess_ReturnSuccess()
	{
		/// Arrange
		CompanyResponse response = new()
		{
			Properties = new()
			{
				HubSpotObjectId = ID,
			},
		};

		_hubSpotClientMock
			.Setup(client => client.GetAsync<CompanyResponse>(
				It.IsAny<RestRequest>(),
				_cancellationToken))
			.ReturnsAsync(new RestResponse<CompanyResponse>()
			{
				IsSuccessStatusCode = true,
				ResponseStatus = ResponseStatus.Completed,
				Data = response
			});

		/// Act
		var result = await _uut.GetByIdAsync("", 0, _cancellationToken);

		/// Assert
		Assert.True(result.IsSuccess);
		Assert.Equal("Id", result.Value.HubSpotId);
	}

	[Fact]
	public async Task GetByIdAsync_ClientReturnsSuccessWithAssociations_ReturnSuccess()
	{
		/// Arrange
		CompanyResponse response = new()
		{
			Properties = new()
			{
				HubSpotObjectId = ID,
			},
		};

		_hubSpotClientMock
			.Setup(client => client.GetAsync<CompanyResponse>(
				It.IsAny<RestRequest>(),
				_cancellationToken))
			.ReturnsAsync(new RestResponse<CompanyResponse>()
			{
				IsSuccessStatusCode = true,
				ResponseStatus = ResponseStatus.Completed,
				Data = response
			});

		/// Act
		var result = await _uut.GetByIdAsync("", 0, _cancellationToken);

		/// Assert
		Assert.True(result.IsSuccess);
		Assert.Equal("Id", result.Value.HubSpotId);
	}

	[Fact]
	public async Task GetAsync_ClientReturnsFailure_ReturnFailure()
	{
		/// Arrange
		_hubSpotClientMock
			.Setup(client => client.GetAsync<CompaniesResponse>(
				It.IsAny<RestRequest>(),
				_cancellationToken))
			.ReturnsAsync(new RestResponse<CompaniesResponse>()
			{
				IsSuccessStatusCode = false,
			});

		/// Act
		var result = await _uut.GetAsync("", _cancellationToken);

		/// Assert
		Assert.True(result.IsFailure);
	}

	[Fact]
	public async Task GetAsync_ClientReturnsSuccess_ReturnSuccess()
	{
		/// Arrange
		CompaniesResponse responses = new()
		{
			Results = new CompanyResponse[]
			{
				new CompanyResponse()
				{
					Properties = new()
					{
						HubSpotObjectId = ID,
					},
				},
			},
		};

		_hubSpotClientMock
			.Setup(client => client.GetAsync<CompaniesResponse>(
				It.IsAny<RestRequest>(),
				_cancellationToken))
			.ReturnsAsync(new RestResponse<CompaniesResponse>()
			{
				IsSuccessStatusCode = true,
				ResponseStatus = ResponseStatus.Completed,
				Data = responses
			});

		/// Act
		var result = await _uut.GetAsync("", _cancellationToken);

		/// Assert
		Assert.True(result.IsSuccess);
		Assert.Equal("Id", result.Value.First().HubSpotId);
	}
}
