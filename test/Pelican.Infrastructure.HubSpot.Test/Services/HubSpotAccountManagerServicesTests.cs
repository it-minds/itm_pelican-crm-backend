using Moq;
using Pelican.Application.Abstractions.Infrastructure;
using Pelican.Domain.Entities;
using Pelican.Domain.Shared;
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

	private readonly Mock<IClient> _hubSpotClientMock = new();
	private readonly HubSpotAccountManagerService _uut;

	public HubSpotAccountManagerServicesTests()
	{
		_uut = new HubSpotAccountManagerService(_hubSpotClientMock.Object);
	}

	[Fact]
	public async Task GetByIdAsync_ClientReturnsFailure_ReturnFailure()
	{
		/// Arrange
		Mock<IResponse<OwnerResponse>> responseMock = new();

		_hubSpotClientMock
			.Setup(client => client.GetAsync<OwnerResponse>(
				It.IsAny<RestRequest>(),
				It.IsAny<CancellationToken>()))
			.ReturnsAsync(responseMock.Object);

		responseMock
			.Setup(r => r.GetResult(It.IsAny<Func<OwnerResponse, AccountManager>>()))
			.Returns(Result.Failure<AccountManager>(Error.NullValue));

		/// Act
		var result = await _uut.GetByUserIdAsync("", 0, default);

		/// Assert
		Assert.True(result.IsFailure);
	}

	[Fact]
	public async Task GetByIdAsync_ClientReturnsSuccess_ReturnSuccess()
	{
		/// Arrange
		Mock<IResponse<OwnerResponse>> responseMock = new();

		AccountManager deal = new();

		_hubSpotClientMock
			.Setup(client => client.GetAsync<OwnerResponse>(
				It.IsAny<RestRequest>(),
				It.IsAny<CancellationToken>()))
			.ReturnsAsync(responseMock.Object);

		responseMock
			.Setup(r => r.GetResult(It.IsAny<Func<OwnerResponse, AccountManager>>()))
			.Returns(deal);

		/// Act
		var result = await _uut.GetByUserIdAsync("", 0, default);

		/// Assert
		Assert.True(result.IsSuccess);
		Assert.Equal(
			deal,
			result.Value);
	}

	[Fact]
	public async Task GetAsync_ClientReturnsFailure_ReturnFailure()
	{
		/// Arrange
		Mock<IResponse<OwnersResponse>> responseMock = new();

		_hubSpotClientMock
			.Setup(client => client.GetAsync<OwnersResponse>(
				It.IsAny<RestRequest>(),
				It.IsAny<CancellationToken>()))
			.ReturnsAsync(responseMock.Object);

		responseMock
			.Setup(r => r.GetResult(It.IsAny<Func<OwnersResponse, List<AccountManager>>>()))
			.Returns(Result.Failure<List<AccountManager>>(Error.NullValue));

		/// Act
		var result = await _uut.GetAsync("", default);

		/// Assert
		Assert.True(result.IsFailure);
	}

	[Fact]
	public async Task GetAsync_ClientReturnsSuccess_ReturnSuccess()
	{
		/// Arrange
		Mock<IResponse<OwnersResponse>> responseMock = new();

		List<AccountManager> deals = new();

		_hubSpotClientMock
			.Setup(client => client.GetAsync<OwnersResponse>(
				It.IsAny<RestRequest>(),
				It.IsAny<CancellationToken>()))
			.ReturnsAsync(responseMock.Object);

		responseMock
			.Setup(r => r.GetResult(It.IsAny<Func<OwnersResponse, List<AccountManager>>>()))
			.Returns(deals);

		/// Act
		var result = await _uut.GetAsync("", default);

		/// Assert
		Assert.True(result.IsSuccess);
		Assert.Equal(
			deals,
			result.Value);
	}
}
