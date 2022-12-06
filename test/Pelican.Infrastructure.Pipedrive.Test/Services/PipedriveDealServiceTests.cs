using Microsoft.Extensions.Options;
using Moq;
using Pelican.Application.Abstractions.Infrastructure;
using Pelican.Application.RestSharp;
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
	private readonly PipedriveDealService _uut;

	public PipedriveDealServiceTests()
	{
		_uut = new(_clientMock.Object);
	}

	[Fact]
	public void PipedriveDealService_ArgumentNull_ThrowException()
	{
		// Act
		var result = Record.Exception(() => new PipedriveDealService(null!));

		// Assert
		Assert.IsType<ArgumentNullException>(result);

		Assert.Contains(
			"client",
			result.Message);
	}


	[Fact]
	public async Task GetByIdAsync_ClientReturnsFailure_ReturnFailure()
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
	public async Task GetByIdAsync_ClientReturnsSuccess_ReturnSuccess()
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
	public async Task GetAsync_ClientReturnsFailure_ReturnFailure()
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
	public async Task GetAsync_ClientReturnsSuccess_ReturnSuccess()
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
