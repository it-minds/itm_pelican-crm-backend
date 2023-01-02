using Moq;
using Pelican.Application.Abstractions.Data.Repositories;
using Pelican.Application.Abstractions.Infrastructure;
using Pelican.Domain.Entities;
using Pelican.Domain.Settings.Pipedrive;
using Pelican.Domain.Shared;
using Pelican.Infrastructure.Pipedrive.Contracts.Responses.Deals;
using Pelican.Infrastructure.Pipedrive.Services;
using RestSharp;
using Xunit;

namespace Pelican.Infrastructure.Pipedrive.Test.Services;

public class PipedriveDealServiceTests
{
	private readonly Mock<IClient<PipedriveSettings>> _clientMock = new();
	private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
	private readonly PipedriveDealService _uut;

	public PipedriveDealServiceTests()
	{
		_uut = new(_clientMock.Object, _unitOfWorkMock.Object);
	}

	[Fact]
	public void PipedriveDealService_ClientArgumentNull_ThrowException()
	{
		// Act
		var result = Record.Exception(() => new PipedriveDealService(null!, _unitOfWorkMock.Object));

		// Assert
		Assert.IsType<ArgumentNullException>(result);

		Assert.Contains(
			"client",
			result.Message);
	}

	[Fact]
	public void PipedriveDealService_UnitOfWorkArgumentNull_ThrowException()
	{
		// Act
		var result = Record.Exception(() => new PipedriveDealService(_clientMock.Object, null!));

		// Assert
		Assert.IsType<ArgumentNullException>(result);

		Assert.Contains(
			"unitOfWork",
			result.Message);
	}

	[Fact]
	public async Task GetByIdAsync_ClientReturnsFailureResult_ReturnFailure()
	{
		/// Arrange
		Mock<IResponse<DealResponse>> responseMock = new();

		_clientMock
			.Setup(client => client.GetAsync<DealResponse>(
				It.IsAny<RestRequest>(),
				It.IsAny<CancellationToken>()))
			.ReturnsAsync(responseMock.Object);

		responseMock
			.Setup(r => r.GetResult(It.IsAny<Func<DealResponse, Deal>>()))
			.Returns(Result.Failure<Deal>(Error.NullValue));

		/// Act
		var result = await _uut.GetByIdAsync("", "", 0, default);

		/// Assert
		Assert.True(result.IsFailure);
	}

	[Fact]
	public async Task GetByIdAsync_ClientReturnsSuccessResult_ReturnSuccess()
	{
		/// Arrange
		Mock<IResponse<DealResponse>> responseMock = new();

		Deal deal = new();

		_clientMock
			.Setup(client => client.GetAsync<DealResponse>(
				It.IsAny<RestRequest>(),
				It.IsAny<CancellationToken>()))
			.ReturnsAsync(responseMock.Object);

		responseMock
			.Setup(r => r.GetResult(It.IsAny<Func<DealResponse, Deal>>()))
			.Returns(deal);

		/// Act
		var result = await _uut.GetByIdAsync("", "", 0, default);

		/// Assert
		Assert.True(result.IsSuccess);
		Assert.Equal(
			deal,
			result.Value);
	}

	[Fact]
	public async Task GetAsync_ClientReturnsFailureResult_ReturnFailure()
	{
		/// Arrange
		Mock<IResponse<DealsResponse>> responseMock = new();

		_clientMock
			.Setup(client => client.GetAsync<DealsResponse>(
				It.IsAny<RestRequest>(),
				It.IsAny<CancellationToken>()))
			.ReturnsAsync(responseMock.Object);

		responseMock
			.Setup(r => r.GetResult(It.IsAny<Func<DealsResponse, List<Deal>>>()))
			.Returns(Result.Failure<List<Deal>>(Error.NullValue));

		/// Act
		var result = await _uut.GetAsync("", "", default);

		/// Assert
		Assert.True(result.IsFailure);
	}

	[Fact]
	public async Task GetAsync_ClientReturnsSuccessResult_ReturnSuccess()
	{
		/// Arrange
		Mock<IResponse<DealsResponse>> responseMock = new();

		List<Deal> deals = new();

		_clientMock
			.Setup(client => client.GetAsync<DealsResponse>(
				It.IsAny<RestRequest>(),
				It.IsAny<CancellationToken>()))
			.ReturnsAsync(responseMock.Object);

		responseMock
			.Setup(r => r.GetResult(It.IsAny<Func<DealsResponse, List<Deal>>>()))
			.Returns(deals);

		/// Act
		var result = await _uut.GetAsync("domain", "", default);

		/// Assert
		Assert.True(result.IsSuccess);
		Assert.Equal(
			deals,
			result.Value);
	}
}
