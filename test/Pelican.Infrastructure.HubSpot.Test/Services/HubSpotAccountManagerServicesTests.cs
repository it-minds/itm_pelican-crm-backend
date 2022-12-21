using Moq;
using Pelican.Application.Abstractions.Data.Repositories;
using Pelican.Application.Abstractions.Infrastructure;
using Pelican.Domain.Entities;
using Pelican.Domain.Settings.HubSpot;
using Pelican.Domain.Shared;
using Pelican.Infrastructure.HubSpot.Contracts.Responses.AccountManagers;
using Pelican.Infrastructure.HubSpot.Contracts.Responses.Common;
using Pelican.Infrastructure.HubSpot.Services;
using RestSharp;
using Xunit;

namespace Pelican.Infrastructure.HubSpot.Test.Services;

public class HubSpotAccountManagerServicesTests
{
	private readonly Mock<IClient<HubSpotSettings>> _hubSpotClientMock = new();
	private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
	private readonly HubSpotAccountManagerService _uut;

	public HubSpotAccountManagerServicesTests()
	{
		_uut = new HubSpotAccountManagerService(_hubSpotClientMock.Object, _unitOfWorkMock.Object);
	}

	[Fact]
	public void HubSpotAccountManagerService_ClientNull_ThrowException()
	{
		// Act
		var result = Record.Exception(() => new HubSpotAccountManagerService(null!, _unitOfWorkMock.Object));

		// Assert
		Assert.IsType<ArgumentNullException>(result);
		Assert.Contains(
			"client",
			result.Message);
	}

	[Fact]
	public void HubSpotAccountManagerService_UnitOfWorkNull_ThrowException()
	{
		// Act
		var result = Record.Exception(() => new HubSpotAccountManagerService(_hubSpotClientMock.Object, null!));

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
		Mock<IResponse<PaginatedResponse<OwnerResponse>>> responseMock = new();

		_hubSpotClientMock
			.Setup(client => client.GetAsync<PaginatedResponse<OwnerResponse>>(
				It.IsAny<RestRequest>(),
				It.IsAny<CancellationToken>()))
			.ReturnsAsync(responseMock.Object);

		responseMock
			.Setup(r => r.GetResult(It.IsAny<Func<PaginatedResponse<OwnerResponse>, List<AccountManager>>>()))
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
		Mock<IResponse<PaginatedResponse<OwnerResponse>>> responseMock = new();

		List<AccountManager> accountManagers = new();

		_hubSpotClientMock
			.Setup(client => client.GetAsync<PaginatedResponse<OwnerResponse>>(
				It.IsAny<RestRequest>(),
				It.IsAny<CancellationToken>()))
			.ReturnsAsync(responseMock.Object);

		responseMock
			.Setup(r => r.GetResult(It.IsAny<Func<PaginatedResponse<OwnerResponse>, List<AccountManager>>>()))
			.Returns(accountManagers);

		/// Act
		var result = await _uut.GetAsync("", default);

		/// Assert
		Assert.True(result.IsSuccess);
		Assert.Equal(
			accountManagers,
			result.Value);
	}

	[Fact]
	public async Task GetAsync_ClientReturnsSuccessTwice_ReturnSuccess()
	{
		/// Arrange
		Mock<IResponse<PaginatedResponse<OwnerResponse>>> responseMock0 = new();
		Mock<IResponse<PaginatedResponse<OwnerResponse>>> responseMock1 = new();

		List<AccountManager> accountManagers = new();

		Paging p = new()
		{
			Next = new()
			{
				After = "1",
			}
		};

		responseMock0
			.Setup(r => r.Data)
			.Returns(new PaginatedResponse<OwnerResponse>()
			{
				Paging = p,
			});

		_hubSpotClientMock
			.SetupSequence(client => client.GetAsync<PaginatedResponse<OwnerResponse>>(
				It.IsAny<RestRequest>(),
				It.IsAny<CancellationToken>()))
			.ReturnsAsync(responseMock0.Object)
			.ReturnsAsync(responseMock1.Object);

		responseMock0
			.Setup(r => r.GetResult(It.IsAny<Func<PaginatedResponse<OwnerResponse>, List<AccountManager>>>()))
			.Returns(accountManagers);

		responseMock1
			.Setup(r => r.GetResult(It.IsAny<Func<PaginatedResponse<OwnerResponse>, List<AccountManager>>>()))
			.Returns(accountManagers);

		/// Act
		var result = await _uut.GetAsync("", default);

		/// Assert
		Assert.True(result.IsSuccess);
		Assert.Equal(
			accountManagers,
			result.Value);
	}
}
