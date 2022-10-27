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

	public HubSpotClientServicesTests()
	{
		_hubSpotClientMock = new();

		_uut = new HubSpotClientService(_hubSpotClientMock.Object);
	}

	[Fact]
	public async Task GetByIdAsync_ClientReturnsNull_ReturnFailure()
	{
		/// Arrange
		RestResponse<CompanyResponse> restResponse = null!;

		_hubSpotClientMock
			.Setup(client => client.GetAsync<CompanyResponse>(
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
		RestResponse<CompanyResponse> restResponse = new()
		{
			IsSuccessStatusCode = false,
		};

		_hubSpotClientMock
			.Setup(client => client.GetAsync<CompanyResponse>(
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
		RestResponse<CompanyResponse> restResponse = new()
		{
			IsSuccessStatusCode = true,
			ResponseStatus = ResponseStatus.Completed,
			Data = null
		};

		_hubSpotClientMock
			.Setup(client => client.GetAsync<CompanyResponse>(
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
		CompanyResponse companyResponse = new();

		RestResponse<CompanyResponse> restResponse = new()
		{
			IsSuccessStatusCode = true,
			ResponseStatus = ResponseStatus.Completed,
			Data = companyResponse
		};

		_hubSpotClientMock
			.Setup(client => client.GetAsync<CompanyResponse>(
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
		CompanyResponse companyResponse = new()
		{
			Properties = new()
			{
				HubSpotObjectId = "id",
				Name = "name",
			}
		};

		RestResponse<CompanyResponse> restResponse = new()
		{
			IsSuccessStatusCode = true,
			ResponseStatus = ResponseStatus.Completed,
			Data = companyResponse
		};

		_hubSpotClientMock
			.Setup(client => client.GetAsync<CompanyResponse>(
				It.IsAny<RestRequest>(),
				default))
			.ReturnsAsync(restResponse);

		/// Act
		var result = await _uut.GetByIdAsync("", 0, default);

		/// Assert
		Assert.True(result.IsSuccess);
		Assert.Equal("id", result.Value.HubSpotId);
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
		RestResponse<CompaniesResponse> restResponse = new()
		{
			IsSuccessStatusCode = false,
		};

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
	public async Task GetAsync_ClientReturnsNullData_ReturnFailure()
	{
		/// Arrange
		RestResponse<CompaniesResponse> restResponse = new()
		{
			IsSuccessStatusCode = true,
			ResponseStatus = ResponseStatus.Completed,
			Data = null
		};

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
	public async Task GetAsync_ClientReturnsEmptyData_ReturnSuccess()
	{
		/// Arrange
		CompaniesResponse companiesResponse = new();

		RestResponse<CompaniesResponse> restResponse = new()
		{
			IsSuccessStatusCode = true,
			ResponseStatus = ResponseStatus.Completed,
			Data = companiesResponse
		};

		_hubSpotClientMock
			.Setup(client => client.GetAsync<CompaniesResponse>(
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
		CompanyResponse companyResponse = new()
		{
			Properties = new()
		};

		CompaniesResponse companiesResponse = new()
		{
			Results = new List<CompanyResponse>() { companyResponse },
		};

		RestResponse<CompaniesResponse> restResponse = new()
		{
			IsSuccessStatusCode = true,
			ResponseStatus = ResponseStatus.Completed,
			Data = companiesResponse
		};

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
	public async Task GetAsync_ClientReturnsValidData_ReturnSuccess()
	{
		/// Arrange
		CompanyResponse companyResponse = new()
		{
			Properties = new()
			{
				HubSpotObjectId = "id",
				Name = "name",
			}
		};

		CompaniesResponse companiesResponse = new()
		{
			Results = new List<CompanyResponse>() { companyResponse },
		};

		RestResponse<CompaniesResponse> restResponse = new()
		{
			IsSuccessStatusCode = true,
			ResponseStatus = ResponseStatus.Completed,
			Data = companiesResponse
		};

		_hubSpotClientMock
			.Setup(client => client.GetAsync<CompaniesResponse>(
				It.IsAny<RestRequest>(),
				default))
			.ReturnsAsync(restResponse);

		/// Act
		var result = await _uut.GetAsync("", default);

		/// Assert
		Assert.True(result.IsSuccess);
		Assert.Equal("id", result.Value.First().HubSpotId);
	}
}
