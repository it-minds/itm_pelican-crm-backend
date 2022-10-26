using Moq;
using Pelican.Domain.Shared;
using Pelican.Infrastructure.HubSpot.Abstractions;
using Pelican.Infrastructure.HubSpot.Contracts.Responses.Deals;
using Pelican.Infrastructure.HubSpot.Services;
using RestSharp;
using Xunit;

namespace Pelican.Infrastructure.HubSpot.Test.Services;

public class HubSpotDealServicesTests
{
	private const string ID = "Id";
	private readonly Mock<IHubSpotClient> _hubSpotClientMock;
	private readonly HubSpotDealService _uut;
	private readonly CancellationToken _cancellationToken;

	public HubSpotDealServicesTests()
	{
		_hubSpotClientMock = new();
		_cancellationToken = new();
		_uut = new HubSpotDealService(_hubSpotClientMock.Object);
	}

	[Fact]
	public async Task GetByIdAsync_ClientReturnsFailure_ReturnFailure()
	{
		const string ERROR = "testError";

		/// Arrange
		_hubSpotClientMock
			.Setup(client => client.GetAsync<DealResponse>(
				It.IsAny<RestRequest>(),
				_cancellationToken))
			.ReturnsAsync(new RestResponse<DealResponse>()
			{
				IsSuccessStatusCode = false,
				StatusCode = System.Net.HttpStatusCode.BadRequest,
				ErrorException = new Exception(ERROR)
			});

		/// Act
		var result = await _uut.GetByIdAsync("", 0, _cancellationToken);

		/// Assert
		Assert.True(result.IsFailure);
		Assert.Equal(ERROR, result.Error.Message);
	}

	[Fact]
	public async Task GetByIdAsync_ClientReturnsNull_ReturnFailureNullError()
	{
		/// Act
		var result = await _uut.GetByIdAsync("", 0, _cancellationToken);

		/// Assert
		Assert.True(result.IsFailure);
		Assert.Equal(Error.NullValue, result.Error);
	}

	[Fact]
	public async Task GetByIdAsync_ClientReturnsSuccess_ReturnSuccess()
	{
		/// Arrange
		DealResponse response = new()
		{
			Properties = new()
			{
				HubSpotObjectId = ID,
			},
		};

		_hubSpotClientMock
			.Setup(client => client.GetAsync<DealResponse>(
				It.IsAny<RestRequest>(),
				_cancellationToken))
			.ReturnsAsync(new RestResponse<DealResponse>()
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
			.Setup(client => client.GetAsync<DealsResponse>(
				It.IsAny<RestRequest>(),
				_cancellationToken))
			.ReturnsAsync(new RestResponse<DealsResponse>()
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
		DealsResponse responses = new()
		{
			Results = new DealResponse[]
			{
				new DealResponse()
				{
					Properties = new()
					{
						HubSpotObjectId = ID,
					},
				},
			},
		};

		_hubSpotClientMock
			.Setup(client => client.GetAsync<DealsResponse>(
				It.IsAny<RestRequest>(),
				_cancellationToken))
			.ReturnsAsync(new RestResponse<DealsResponse>()
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
