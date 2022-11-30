using Moq;
using Pelican.Application.Abstractions.Infrastructure;
using Pelican.Infrastructure.HubSpot.Abstractions;
using Pelican.Infrastructure.HubSpot.Contracts.Responses.Contacts;
using Pelican.Infrastructure.HubSpot.Services;
using RestSharp;
using Xunit;

namespace Pelican.Infrastructure.HubSpot.Test.Services;

public class HubSpotContactServicesTests
{
	private const string ID = "Id";
	private const string OWNERID = "OwnerId";

	private readonly Mock<IClient> _hubSpotClientMock;
	private readonly HubSpotContactService _uut;

	public HubSpotContactServicesTests()
	{
		_hubSpotClientMock = new();
		_uut = new HubSpotContactService(_hubSpotClientMock.Object);
	}

	[Fact]
	public async Task GetByIdAsync_ClientReturnsNull_ReturnFailure()
	{
		/// Arrange
		RestResponse<ContactResponse> restResponse = null!;

		_hubSpotClientMock
			.Setup(client => client.GetAsync<ContactResponse>(
				It.IsAny<RestRequest>(),
				default))
			.ReturnsAsync(restResponse);

		/// Act
		var result = await _uut.GetByIdAsync("", 0, default);

		/// Assert
		Assert.True(result.IsFailure);
	}

	[Fact]
	public async Task GetByIdAsync_ClientReturnsFailure_ReturnFailure()
	{
		/// Arrange
		RestResponse<ContactResponse> restResponse = new()
		{
			IsSuccessStatusCode = false,
		};

		_hubSpotClientMock
			.Setup(client => client.GetAsync<ContactResponse>(
				It.IsAny<RestRequest>(),
				default))
			.ReturnsAsync(restResponse);

		/// Act
		var result = await _uut.GetByIdAsync("", 0, default);

		/// Assert
		Assert.True(result.IsFailure);
	}

	[Fact]
	public async Task GetByIdAsync_ClientReturnsNullData_ReturnFailure()
	{
		/// Arrange
		RestResponse<ContactResponse> restResponse = new()
		{
			IsSuccessStatusCode = true,
			ResponseStatus = ResponseStatus.Completed,
			Data = null
		};

		_hubSpotClientMock
			.Setup(client => client.GetAsync<ContactResponse>(
				It.IsAny<RestRequest>(),
				default))
			.ReturnsAsync(restResponse);

		/// Act
		var result = await _uut.GetByIdAsync("", 0, default);

		/// Assert
		Assert.True(result.IsFailure);
	}

	[Fact]
	public async Task GetByIdAsync_ClientReturnsInvalidData_ReturnFailure()
	{
		/// Arrange
		ContactResponse contactResponse = new();

		RestResponse<ContactResponse> restResponse = new()
		{
			IsSuccessStatusCode = true,
			ResponseStatus = ResponseStatus.Completed,
			Data = contactResponse
		};

		_hubSpotClientMock
			.Setup(client => client.GetAsync<ContactResponse>(
				It.IsAny<RestRequest>(),
				default))
			.ReturnsAsync(restResponse);

		/// Act
		var result = await _uut.GetByIdAsync("", 0, default);

		/// Assert
		Assert.True(result.IsFailure);
	}

	[Fact]
	public async Task GetByIdAsync_ClientReturnsValidData_ReturnSuccess()
	{
		/// Arrange
		ContactResponse contactResponse = new()
		{
			Properties = new()
			{
				HubSpotObjectId = ID,
				HubSpotOwnerId = OWNERID,
			}
		};

		RestResponse<ContactResponse> restResponse = new()
		{
			IsSuccessStatusCode = true,
			ResponseStatus = ResponseStatus.Completed,
			Data = contactResponse
		};

		_hubSpotClientMock
			.Setup(client => client.GetAsync<ContactResponse>(
				It.IsAny<RestRequest>(),
				default))
			.ReturnsAsync(restResponse);

		/// Act
		var result = await _uut.GetByIdAsync("", 0, default);

		/// Assert
		Assert.True(result.IsSuccess);
		Assert.Equal(ID, result.Value.HubSpotId);
		Assert.Equal(OWNERID, result.Value.HubSpotOwnerId);
	}

	[Fact]
	public async Task GetAsync_ClientReturnsNull_ReturnFailure()
	{
		/// Arrange
		RestResponse<ContactsResponse> restResponse = null!;

		_hubSpotClientMock
			.Setup(client => client.GetAsync<ContactsResponse>(
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
		RestResponse<ContactsResponse> restResponse = new()
		{
			IsSuccessStatusCode = false,
		};

		_hubSpotClientMock
			.Setup(client => client.GetAsync<ContactsResponse>(
				It.IsAny<RestRequest>(),
				default))
			.ReturnsAsync(restResponse);

		/// Act
		var result = await _uut.GetAsync("", default);

		/// Assert
		Assert.True(result.IsFailure);
	}

	[Fact]
	public async Task GetAsync_ClientReturnsNullData_ReturnFailure()
	{
		/// Arrange
		RestResponse<ContactsResponse> restResponse = new()
		{
			IsSuccessStatusCode = true,
			ResponseStatus = ResponseStatus.Completed,
			Data = null
		};

		_hubSpotClientMock
			.Setup(client => client.GetAsync<ContactsResponse>(
				It.IsAny<RestRequest>(),
				default))
			.ReturnsAsync(restResponse);

		/// Act
		var result = await _uut.GetAsync("", default);

		/// Assert
		Assert.True(result.IsFailure);
	}

	[Fact]
	public async Task GetAsync_ClientReturnsEmptyData_ReturnSuccess()
	{
		/// Arrange
		ContactsResponse contactsResponse = new();

		RestResponse<ContactsResponse> restResponse = new()
		{
			IsSuccessStatusCode = true,
			ResponseStatus = ResponseStatus.Completed,
			Data = contactsResponse
		};

		_hubSpotClientMock
			.Setup(client => client.GetAsync<ContactsResponse>(
				It.IsAny<RestRequest>(),
				default))
			.ReturnsAsync(restResponse);

		/// Act
		var result = await _uut.GetAsync("", default);

		/// Assert
		Assert.True(result.IsSuccess);
	}

	[Fact]
	public async Task GetAsync_ClientReturnsInvalidData_ReturnFailure()
	{
		/// Arrange
		ContactResponse contactResponse = new()
		{
			Properties = new()
		};

		ContactsResponse contactsResponse = new()
		{
			Results = new List<ContactResponse>() { contactResponse },
		};

		RestResponse<ContactsResponse> restResponse = new()
		{
			IsSuccessStatusCode = true,
			ResponseStatus = ResponseStatus.Completed,
			Data = contactsResponse
		};

		_hubSpotClientMock
			.Setup(client => client.GetAsync<ContactsResponse>(
				It.IsAny<RestRequest>(),
				default))
			.ReturnsAsync(restResponse);

		/// Act
		var result = await _uut.GetAsync("", default);

		/// Assert
		Assert.True(result.IsFailure);
	}

	[Fact]
	public async Task GetAsync_ClientReturnsValidData_ReturnSuccess()
	{
		/// Arrange
		ContactResponse contactResponse = new()
		{
			Properties = new()
			{
				HubSpotObjectId = ID,
				HubSpotOwnerId = OWNERID,
			}
		};

		ContactsResponse contactsResponse = new()
		{
			Results = new List<ContactResponse>() { contactResponse },
		};

		RestResponse<ContactsResponse> restResponse = new()
		{
			IsSuccessStatusCode = true,
			ResponseStatus = ResponseStatus.Completed,
			Data = contactsResponse
		};

		_hubSpotClientMock
			.Setup(client => client.GetAsync<ContactsResponse>(
				It.IsAny<RestRequest>(),
				default))
			.ReturnsAsync(restResponse);

		/// Act
		var result = await _uut.GetAsync("", default);

		/// Assert
		Assert.True(result.IsSuccess);
		Assert.Equal(ID, result.Value.First().HubSpotId);
		Assert.Equal(OWNERID, result.Value.First().HubSpotOwnerId);
	}
}
