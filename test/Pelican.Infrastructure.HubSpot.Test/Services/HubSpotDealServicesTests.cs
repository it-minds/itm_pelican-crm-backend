using Moq;
using Pelican.Application.Abstractions.Infrastructure;
using Pelican.Infrastructure.HubSpot.Contracts.Responses.Deals;
using Pelican.Infrastructure.HubSpot.Services;
using RestSharp;
using Xunit;

namespace Pelican.Infrastructure.HubSpot.Test.Services;

public class HubSpotDealServicesTests
{
	private const string ID = "Id";
	private const string OWNERID = "OwnerId";

	private readonly Mock<IClient> _hubSpotClientMock;
	private readonly HubSpotDealService _uut;

	public HubSpotDealServicesTests()
	{
		_hubSpotClientMock = new();
		_uut = new HubSpotDealService(_hubSpotClientMock.Object);
	}

	[Fact]
	public async Task GetByIdAsync_ClientReturnsFailure_ReturnFailure()
	{
		/// Arrange
		IResponse<DealResponse> RestSharpResponse = new RestSharpResponse<DealResponse>()
		{
			IsSuccessStatusCode = false,
		};

		_hubSpotClientMock
			.Setup(client => client.GetAsync<DealResponse>(
				It.IsAny<RestRequest>(),
				default))
			.ReturnsAsync(RestSharpResponse);

		/// Act
		var result = await _uut.GetByIdAsync("", 0, default);

		/// Assert
		Assert.True(result.IsFailure);
	}

	[Fact]
	public async Task GetByIdAsync_ClientReturnsNullData_ReturnFailure()
	{
		/// Arrange
		RestSharpResponse<DealResponse> RestSharpResponse = new()
		{
			IsSuccessStatusCode = true,
			ResponseStatus = ResponseStatus.Completed,
			Data = null
		};

		_hubSpotClientMock
			.Setup(client => client.GetAsync<DealResponse>(
				It.IsAny<RestRequest>(),
				default))
			.ReturnsAsync(RestSharpResponse);

		/// Act
		var result = await _uut.GetByIdAsync("", 0, default);

		/// Assert
		Assert.True(result.IsFailure);
	}

	[Fact]
	public async Task GetByIdAsync_ClientReturnsInvalidData_ReturnFailure()
	{
		/// Arrange
		DealResponse dealResponse = new();

		RestSharpResponse<DealResponse> RestSharpResponse = new()
		{
			IsSuccessStatusCode = true,
			ResponseStatus = ResponseStatus.Completed,
			Data = dealResponse
		};

		_hubSpotClientMock
			.Setup(client => client.GetAsync<DealResponse>(
				It.IsAny<RestRequest>(),
				default))
			.ReturnsAsync(RestSharpResponse);

		/// Act
		var result = await _uut.GetByIdAsync("", 0, default);

		/// Assert
		Assert.True(result.IsFailure);
	}

	[Fact]
	public async Task GetByIdAsync_ClientReturnsValidData_ReturnSuccess()
	{
		/// Arrange
		DealResponse dealResponse = new()
		{
			Properties = new()
			{
				HubSpotObjectId = ID,
				HubSpotOwnerId = OWNERID,
			}
		};

		RestSharpResponse<DealResponse> RestSharpResponse = new()
		{
			IsSuccessStatusCode = true,
			ResponseStatus = ResponseStatus.Completed,
			Data = dealResponse
		};

		_hubSpotClientMock
			.Setup(client => client.GetAsync<DealResponse>(
				It.IsAny<RestRequest>(),
				default))
			.ReturnsAsync(RestSharpResponse);

		/// Act
		var result = await _uut.GetByIdAsync("", 0, default);

		/// Assert
		Assert.True(result.IsSuccess);
		Assert.Equal(ID, result.Value.HubSpotId);
		Assert.Equal(OWNERID, result.Value.HubSpotOwnerId);
	}

	[Fact]
	public async Task GetAsync_ClientReturnsFailure_ReturnFailure()
	{
		/// Arrange
		RestSharpResponse<DealsResponse> RestSharpResponse = new()
		{
			IsSuccessStatusCode = false,
		};

		_hubSpotClientMock
			.Setup(client => client.GetAsync<DealsResponse>(
				It.IsAny<RestRequest>(),
				default))
			.ReturnsAsync(RestSharpResponse);

		/// Act
		var result = await _uut.GetAsync("", default);

		/// Assert
		Assert.True(result.IsFailure);
	}

	[Fact]
	public async Task GetAsync_ClientReturnsNullData_ReturnFailure()
	{
		/// Arrange
		RestSharpResponse<DealsResponse> RestSharpResponse = new()
		{
			IsSuccessStatusCode = true,
			ResponseStatus = ResponseStatus.Completed,
			Data = null
		};

		_hubSpotClientMock
			.Setup(client => client.GetAsync<DealsResponse>(
				It.IsAny<RestRequest>(),
				default))
			.ReturnsAsync(RestSharpResponse);

		/// Act
		var result = await _uut.GetAsync("", default);

		/// Assert
		Assert.True(result.IsFailure);
	}

	[Fact]
	public async Task GetAsync_ClientReturnsEmptyData_ReturnSuccess()
	{
		/// Arrange
		DealsResponse dealsResponse = new();

		RestSharpResponse<DealsResponse> RestSharpResponse = new()
		{
			IsSuccessStatusCode = true,
			ResponseStatus = ResponseStatus.Completed,
			Data = dealsResponse
		};

		_hubSpotClientMock
			.Setup(client => client.GetAsync<DealsResponse>(
				It.IsAny<RestRequest>(),
				default))
			.ReturnsAsync(RestSharpResponse);

		/// Act
		var result = await _uut.GetAsync("", default);

		/// Assert
		Assert.True(result.IsSuccess);
	}

	[Fact]
	public async Task GetAsync_ClientReturnsInvalidData_ReturnFailure()
	{
		/// Arrange
		DealResponse dealResponse = new()
		{
			Properties = new()
		};

		DealsResponse dealsResponse = new()
		{
			Results = new List<DealResponse>() { dealResponse },
		};

		RestSharpResponse<DealsResponse> RestSharpResponse = new()
		{
			IsSuccessStatusCode = true,
			ResponseStatus = ResponseStatus.Completed,
			Data = dealsResponse
		};

		_hubSpotClientMock
			.Setup(client => client.GetAsync<DealsResponse>(
				It.IsAny<RestRequest>(),
				default))
			.ReturnsAsync(RestSharpResponse);

		/// Act
		var result = await _uut.GetAsync("", default);

		/// Assert
		Assert.True(result.IsFailure);
	}

	[Fact]
	public async Task GetAsync_ClientReturnsValidData_ReturnSuccess()
	{
		/// Arrange
		DealResponse dealResponse = new()
		{
			Properties = new()
			{
				HubSpotObjectId = ID,
				HubSpotOwnerId = OWNERID,
			}
		};

		DealsResponse dealsResponse = new()
		{
			Results = new List<DealResponse>() { dealResponse },
		};

		RestSharpResponse<DealsResponse> RestSharpResponse = new()
		{
			IsSuccessStatusCode = true,
			ResponseStatus = ResponseStatus.Completed,
			Data = dealsResponse
		};

		_hubSpotClientMock
			.Setup(client => client.GetAsync<DealsResponse>(
				It.IsAny<RestRequest>(),
				default))
			.ReturnsAsync(RestSharpResponse);

		/// Act
		var result = await _uut.GetAsync("", default);

		/// Assert
		Assert.True(result.IsSuccess);
		Assert.Equal(ID, result.Value.First().HubSpotId);
		Assert.Equal(OWNERID, result.Value.First().HubSpotOwnerId);
	}
}
