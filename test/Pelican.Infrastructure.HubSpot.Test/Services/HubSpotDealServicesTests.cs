using Moq;
using Pelican.Application.Abstractions.Data.Repositories;
using Pelican.Application.Abstractions.Infrastructure;
using Pelican.Domain.Entities;
using Pelican.Domain.Settings.HubSpot;
using Pelican.Domain.Shared;
using Pelican.Infrastructure.HubSpot.Contracts.Responses.Clients;
using Pelican.Infrastructure.HubSpot.Contracts.Responses.Common;
using Pelican.Infrastructure.HubSpot.Contracts.Responses.Deals;
using Pelican.Infrastructure.HubSpot.Services;
using RestSharp;
using Xunit;

namespace Pelican.Infrastructure.HubSpot.Test.Services;

public class HubSpotDealServicesTests
{
	private readonly Mock<IClient<HubSpotSettings>> _hubSpotClientMock = new();
	private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
	private readonly HubSpotDealService _uut;

	public HubSpotDealServicesTests()
	{
		_uut = new HubSpotDealService(
			_hubSpotClientMock.Object,
			_unitOfWorkMock.Object);
	}

	[Fact]
	public void HubSpotDealService_ClientNull_ThrowException()
	{
		// Act
		var result = Record.Exception(() => new HubSpotDealService(null!, _unitOfWorkMock.Object));

		// Assert
		Assert.IsType<ArgumentNullException>(result);
		Assert.Contains(
			"client",
			result.Message);
	}

	[Fact]
	public void HubSpotDealService_UnitOfWorkNull_ThrowException()
	{
		// Act
		var result = Record.Exception(() => new HubSpotDealService(_hubSpotClientMock.Object, null!));

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
		Mock<IResponse<DealResponse>> responseMock = new();

		_hubSpotClientMock
			.Setup(client => client.GetAsync<DealResponse>(
				It.IsAny<RestRequest>(),
				It.IsAny<CancellationToken>()))
			.ReturnsAsync(responseMock.Object);

		responseMock
			.Setup(r => r.GetResultWithUnitOfWork(
				It.IsAny<Func<DealResponse, IUnitOfWork, CancellationToken, Task<Deal>>>(),
				It.IsAny<IUnitOfWork>(),
				It.IsAny<CancellationToken>()))
			.ReturnsAsync(Result.Failure<Deal>(Error.NullValue));

		/// Act
		var result = await _uut.GetByIdAsync("", 0, default);

		/// Assert
		Assert.True(result.IsFailure);
	}

	[Fact]
	public async Task GetByIdAsync_ClientReturnsSuccess_ReturnSuccess()
	{
		/// Arrange
		Mock<IResponse<DealResponse>> responseMock = new();

		Deal deal = new();

		_hubSpotClientMock
			.Setup(client => client.GetAsync<DealResponse>(
				It.IsAny<RestRequest>(),
				It.IsAny<CancellationToken>()))
			.ReturnsAsync(responseMock.Object);

		responseMock
			.Setup(r => r.GetResultWithUnitOfWork(
				It.IsAny<Func<DealResponse, IUnitOfWork, CancellationToken, Task<Deal>>>(),
				It.IsAny<IUnitOfWork>(),
				It.IsAny<CancellationToken>()))
			.ReturnsAsync(Result.Success(deal));

		/// Act
		var result = await _uut.GetByIdAsync("", 0, default);

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
		Mock<IResponse<PaginatedResponse<DealResponse>>> responseMock = new();

		_hubSpotClientMock
			.Setup(client => client.GetAsync<PaginatedResponse<DealResponse>>(
				It.IsAny<RestRequest>(),
				It.IsAny<CancellationToken>()))
			.ReturnsAsync(responseMock.Object);

		responseMock
			.Setup(r => r.GetResultWithUnitOfWork(
				It.IsAny<Func<PaginatedResponse<DealResponse>, IUnitOfWork, CancellationToken, Task<List<Deal>>>>(),
				It.IsAny<IUnitOfWork>(),
				It.IsAny<CancellationToken>()))
			.ReturnsAsync(Result.Failure<List<Deal>>(Error.NullValue));

		/// Act
		var result = await _uut.GetAsync("", default);

		/// Assert
		Assert.True(result.IsFailure);
	}

	[Fact]
	public async Task GetAsync_ClientReturnsSuccess_ReturnSuccess()
	{
		/// Arrange
		Mock<IResponse<PaginatedResponse<DealResponse>>> responseMock = new();

		List<Deal> deals = new();

		_hubSpotClientMock
			.Setup(client => client.GetAsync<PaginatedResponse<DealResponse>>(
				It.IsAny<RestRequest>(),
				It.IsAny<CancellationToken>()))
			.ReturnsAsync(responseMock.Object);

		responseMock
			.Setup(r => r.GetResultWithUnitOfWork(
				It.IsAny<Func<PaginatedResponse<DealResponse>, IUnitOfWork, CancellationToken, Task<List<Deal>>>>(),
				It.IsAny<IUnitOfWork>(),
				It.IsAny<CancellationToken>()))
			.ReturnsAsync(Result.Success(deals));

		/// Act
		var result = await _uut.GetAsync("", default);

		/// Assert
		Assert.True(result.IsSuccess);
		Assert.Equal(
			deals,
			result.Value);
	}

	[Fact]
	public async Task GetAsync_ClientReturnsSuccessTwice_ReturnSuccess()
	{
		/// Arrange
		Mock<IResponse<PaginatedResponse<DealResponse>>> responseMock0 = new();
		Mock<IResponse<PaginatedResponse<DealResponse>>> responseMock1 = new();

		List<Deal> deals = new();

		Paging p = new()
		{
			Next = new()
			{
				After = "1",
			}
		};

		responseMock0
			.Setup(r => r.Data)
			.Returns(new PaginatedResponse<DealResponse>()
			{
				Paging = p,
			});

		_hubSpotClientMock
			.SetupSequence(client => client.GetAsync<PaginatedResponse<DealResponse>>(
				It.IsAny<RestRequest>(),
				It.IsAny<CancellationToken>()))
			.ReturnsAsync(responseMock0.Object)
			.ReturnsAsync(responseMock1.Object);

		responseMock0
			.Setup(r => r.GetResultWithUnitOfWork(
				It.IsAny<Func<PaginatedResponse<DealResponse>, IUnitOfWork, CancellationToken, Task<List<Deal>>>>(),
				It.IsAny<IUnitOfWork>(),
				It.IsAny<CancellationToken>()))
			.ReturnsAsync(deals);

		responseMock1
			.Setup(r => r.GetResultWithUnitOfWork(
				It.IsAny<Func<PaginatedResponse<DealResponse>, IUnitOfWork, CancellationToken, Task<List<Deal>>>>(),
				It.IsAny<IUnitOfWork>(),
				It.IsAny<CancellationToken>()))
			.ReturnsAsync(deals);

		/// Act
		var result = await _uut.GetAsync("", default);

		/// Assert
		Assert.True(result.IsSuccess);
		Assert.Equal(
			deals,
			result.Value);
	}
}
