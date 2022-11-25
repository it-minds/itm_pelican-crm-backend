using Moq;
using Pelican.Infrastructure.HubSpot.Abstractions;
using Pelican.Infrastructure.HubSpot.Contracts.Responses.AccountManagers;
using Pelican.Infrastructure.HubSpot.Services;
using RestSharp;
using Xunit;

namespace Pelican.Infrastructure.HubSpot.Test.Services;

public class HubSpotAccountManagerServicesTests
{
	private const string ID = "Id";
	private const string EMAIL = "Email";
	private const string FIRSTNAME = "Firstname";
	private const string LASTNAME = "Lastname";

	private readonly Mock<IHubSpotClient> _hubSpotClientMock = new();
	private readonly HubSpotAccountManagerService _uut;

	public HubSpotAccountManagerServicesTests()
	{
		_uut = new HubSpotAccountManagerService(_hubSpotClientMock.Object);
	}

	[Fact]
	public async Task GetByUserIdAsync_ClientReturnsFailure_ReturnFailure()
	{
		// Arrange
		_hubSpotClientMock
			.Setup(client => client.GetAsync<OwnerResponse>(
				It.IsAny<RestRequest>(),
				It.IsAny<CancellationToken>()))
			.ReturnsAsync(new RestResponse<OwnerResponse>()
			{
				IsSuccessStatusCode = false,
			});

		// Act
		var result = await _uut.GetByUserIdAsync("", 0, default);

		// Assert
		Assert.True(result.IsFailure);
	}

	[Fact]
	public async Task GetByUserIdAsync_ClientReturnsSuccess_ReturnSuccess()
	{
		// Arrange
		OwnerResponse response = new()
		{
			Id = ID,
			Email = EMAIL,
			Firstname = FIRSTNAME,
			Lastname = LASTNAME,
		};

		_hubSpotClientMock
			.Setup(client => client.GetAsync<OwnerResponse>(
				It.IsAny<RestRequest>(),
				It.IsAny<CancellationToken>()))
			.ReturnsAsync(new RestResponse<OwnerResponse>()
			{
				IsSuccessStatusCode = true,
				ResponseStatus = ResponseStatus.Completed,
				Data = response
			});

		// Act
		var result = await _uut.GetByUserIdAsync("", 0, default);

		// Assert
		Assert.True(result.IsSuccess);
		Assert.Equal("Id", result.Value.HubSpotId);
	}

	[Fact]
	public async Task GetAsync_ClientReturnsFailure_ReturnFailure()
	{
		// Arrange
		_hubSpotClientMock
			.Setup(client => client.GetAsync<OwnersResponse>(
				It.IsAny<RestRequest>(),
				It.IsAny<CancellationToken>()))
			.ReturnsAsync(new RestResponse<OwnersResponse>()
			{
				IsSuccessStatusCode = false,
			});

		// Act
		var result = await _uut.GetAsync("", default);

		// Assert
		Assert.True(result.IsFailure);
	}

	[Fact]
	public async Task GetAsync_ClientReturnsSuccess_ReturnSuccess()
	{
		// Arrange
		OwnersResponse responses = new()
		{
			Results = new OwnerResponse[]
			{
				new OwnerResponse()
				{
					Id = ID,
					Email = EMAIL,
					Firstname = FIRSTNAME,
					Lastname = LASTNAME,
				},
			},
		};

		_hubSpotClientMock
			.Setup(client => client.GetAsync<OwnersResponse>(
				It.IsAny<RestRequest>(),
				It.IsAny<CancellationToken>()))
			.ReturnsAsync(new RestResponse<OwnersResponse>()
			{
				IsSuccessStatusCode = true,
				ResponseStatus = ResponseStatus.Completed,
				Data = responses
			});

		// Act
		var result = await _uut.GetAsync("", default);

		// Assert
		Assert.True(result.IsSuccess);
		Assert.Equal("Id", result.Value.First().HubSpotId);
	}
}
