using System.Linq.Expressions;
using Microsoft.Extensions.Options;
using Moq;
using Pelican.Application.Abstractions.Data.Repositories;
using Pelican.Application.Abstractions.Infrastructure;
using Pelican.Application.HubSpot.Dtos;
using Pelican.Domain.Entities;
using Pelican.Domain.Settings.HubSpot;
using Pelican.Domain.Shared;
using Pelican.Infrastructure.HubSpot.Contracts.Responses.Auth;
using Pelican.Infrastructure.HubSpot.Services;
using RestSharp;
using Xunit;

namespace Pelican.Infrastructure.HubSpot.Test.Services;

public class HubSpotAuthorizationServicesTests
{
	private const string REDIRECTURL = "redirectUrl";
	private const string BASEURL = "baseUrl";
	private const int APPID = 123;
	private const string CLIENTID = "clientId";
	private const string CLIENTSECRET = "clientSecret";
	private const string ACCESS = "access";
	private const string REFRESH = "refresh";

	private readonly Mock<IClient> _hubSpotClientMock = new();
	private readonly Mock<IOptions<HubSpotSettings>> _optionsMock = new();
	private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
	private readonly HubSpotAuthorizationService _uut;

	public HubSpotAuthorizationServicesTests()
	{
		_optionsMock
			.Setup(options => options.Value)
			.Returns(new HubSpotSettings()
			{
				RedirectUrl = REDIRECTURL,
				BaseUrl = BASEURL,
				App = new HubSpotAppSettings()
				{
					AppId = APPID,
					ClientId = CLIENTID,
					ClientSecret = CLIENTSECRET,
				},
			});

		_uut = new(
			_hubSpotClientMock.Object,
			_optionsMock.Object);
	}

	[Fact]
	public async Task AuthorizeUserAsync_ClientReturnsFailure_ReturnFailure()
	{
		// Arrange
		Mock<IResponse<GetAccessTokenResponse>> responseMock = new();

		responseMock
			.Setup(r => r.GetResult(It.IsAny<Func<GetAccessTokenResponse, RefreshAccessTokens>>()))
			.Returns(Result.Failure<RefreshAccessTokens>(Error.NullValue));

		_hubSpotClientMock
			.Setup(client => client.PostAsync<GetAccessTokenResponse>(
				It.IsAny<RestRequest>(),
				It.IsAny<CancellationToken>()))
			.ReturnsAsync(responseMock.Object);

		// Act
		var result = await _uut.AuthorizeUserAsync("", default);

		// Assert
		Assert.True(result.IsFailure);
		Assert.Equal(Error.NullValue, result.Error);
	}

	[Fact]
	public async Task AuthorizeUserAsync_ClientReturnsSuccess_ReturnSuccess()
	{
		// Arrange
		Mock<IResponse<GetAccessTokenResponse>> responseMock = new();

		responseMock
			.Setup(r => r.GetResult(It.IsAny<Func<GetAccessTokenResponse, RefreshAccessTokens>>()))
			.Returns(Result.Success(new RefreshAccessTokens()));

		_hubSpotClientMock
			.Setup(client => client.PostAsync<GetAccessTokenResponse>(
				It.IsAny<RestRequest>(),
				It.IsAny<CancellationToken>()))
			.ReturnsAsync(responseMock.Object);

		// Act
		var result = await _uut.AuthorizeUserAsync("", default);

		// Assert
		Assert.True(result.IsSuccess);
	}

	[Fact]
	public async Task DecodeAccessTokenAsync_ClientReturnsFailure_ReturnFailure()
	{
		// Arrange
		Mock<IResponse<AccessTokenResponse>> responseMock = new();

		responseMock
			.Setup(r => r.GetResult(It.IsAny<Func<AccessTokenResponse, Supplier>>()))
			.Returns(Result.Failure<Supplier>(Error.NullValue));

		_hubSpotClientMock
			.Setup(client => client.GetAsync<AccessTokenResponse>(
				It.IsAny<RestRequest>(),
				It.IsAny<CancellationToken>()))
			.ReturnsAsync(responseMock.Object);

		// Act
		var result = await _uut.DecodeAccessTokenAsync("", default);

		// Assert
		Assert.True(result.IsFailure);
		Assert.Equal(Error.NullValue, result.Error);
	}

	[Fact]
	public async Task DecodeAccessTokenAsync_ClientReturnsSuccess_ReturnSuccess()
	{
		// Arrange
		Mock<IResponse<AccessTokenResponse>> responseMock = new();

		responseMock
			.Setup(r => r.GetResult(It.IsAny<Func<AccessTokenResponse, Supplier>>()))
			.Returns(Result.Success(new Supplier()));

		_hubSpotClientMock
			.Setup(client => client.GetAsync<AccessTokenResponse>(
				It.IsAny<RestRequest>(),
				It.IsAny<CancellationToken>()))
			.ReturnsAsync(responseMock.Object);

		// Act
		var result = await _uut.DecodeAccessTokenAsync("", default);

		// Assert
		Assert.True(result.IsSuccess);
	}

	[Fact]
	public async Task RefreshAccessTokenFromSupplierHubSpotIdAsync_SupplierDoesNotExist_ReturnFailure()
	{
		// Arrange
		_unitOfWorkMock
			.Setup(m => m
				.SupplierRepository
				.FindByCondition(It.IsAny<Expression<Func<Supplier, bool>>>()))
			.Returns(Enumerable.Empty<Supplier>().AsQueryable());

		// Act
		var result = await _uut.RefreshAccessTokenFromSupplierHubSpotIdAsync(1, _unitOfWorkMock.Object, default);

		// Assert
		Assert.True(result.IsFailure);
		Assert.Equal(Error.NullValue, result.Error);
	}

	[Fact]
	public async Task RefreshAccessTokenFromSupplierHubSpotIdAsync_SupplierRefreshTokenNull_ReturnFailure()
	{
		//Arrange
		Supplier supplier = new();

		_unitOfWorkMock
			.Setup(m => m
				.SupplierRepository
				.FindByCondition(It.IsAny<Expression<Func<Supplier, bool>>>()))
			.Returns(new List<Supplier> { supplier }.AsQueryable());

		// Act
		var result = await _uut.RefreshAccessTokenFromSupplierHubSpotIdAsync(1, _unitOfWorkMock.Object, default);

		// Assert
		Assert.True(result.IsFailure);
		Assert.Equal(Error.NullValue, result.Error);
	}

	[Fact]
	public async Task RefreshAccessTokenFromSupplierHubSpotIdAsync_ClientReturnsFailure_ReturnFailure()
	{
		// Arrange
		Supplier supplier = new() { RefreshToken = REFRESH };

		_unitOfWorkMock
			.Setup(m => m
				.SupplierRepository
				.FirstOrDefaultAsync(It.IsAny<Expression<Func<Supplier, bool>>>(), It.IsAny<CancellationToken>()))
			.ReturnsAsync(supplier);

		Mock<IResponse<RefreshAccessTokenResponse>> responseMock = new();

		responseMock
			.Setup(r => r.GetResult(It.IsAny<Func<RefreshAccessTokenResponse, string>>()))
			.Returns(Result.Failure<string>(Error.NullValue));

		_hubSpotClientMock
			.Setup(client => client.PostAsync<RefreshAccessTokenResponse>(
				It.IsAny<RestRequest>(),
				It.IsAny<CancellationToken>()))
			.ReturnsAsync(responseMock.Object);

		// Act
		var result = await _uut.RefreshAccessTokenFromSupplierHubSpotIdAsync(0, _unitOfWorkMock.Object, default);

		// Assert
		Assert.True(result.IsFailure);
		Assert.Equal(Error.NullValue, result.Error);
	}

	[Fact]
	public async Task RefreshAccessTokenFromSupplierHubSpotIdAsync_ClientReturnsSuccess_ReturnSuccess()
	{
		// Arrange
		Supplier supplier = new() { RefreshToken = REFRESH };

		_unitOfWorkMock
			.Setup(m => m
				.SupplierRepository
				.FirstOrDefaultAsync(It.IsAny<Expression<Func<Supplier, bool>>>(), It.IsAny<CancellationToken>()))
			.ReturnsAsync(supplier);

		Mock<IResponse<RefreshAccessTokenResponse>> responseMock = new();

		responseMock
			.Setup(r => r.GetResult(It.IsAny<Func<RefreshAccessTokenResponse, string>>()))
			.Returns(Result.Success(ACCESS));

		_hubSpotClientMock
			.Setup(client => client.PostAsync<RefreshAccessTokenResponse>(
				It.IsAny<RestRequest>(),
				It.IsAny<CancellationToken>()))
			.ReturnsAsync(responseMock.Object);

		// Act
		var result = await _uut.RefreshAccessTokenFromSupplierHubSpotIdAsync(0, _unitOfWorkMock.Object, default);

		// Assert
		Assert.True(result.IsSuccess);
		Assert.Equal(ACCESS, result.Value);
	}

	[Fact]
	public async Task RefreshAccessTokenFromRefreshTokenAsync_ClientReturnsFailure_ReturnFailure()
	{
		// Arrange
		Mock<IResponse<RefreshAccessTokenResponse>> responseMock = new();

		responseMock
			.Setup(r => r.GetResult(It.IsAny<Func<RefreshAccessTokenResponse, string>>()))
			.Returns(Result.Failure<string>(Error.NullValue));

		_hubSpotClientMock
			.Setup(client => client.PostAsync<RefreshAccessTokenResponse>(
				It.IsAny<RestRequest>(),
				It.IsAny<CancellationToken>()))
			.ReturnsAsync(responseMock.Object);

		// Act
		var result = await _uut.RefreshAccessTokenFromRefreshTokenAsync("", default);

		// Assert
		Assert.True(result.IsFailure);
		Assert.Equal(Error.NullValue, result.Error);
	}

	[Fact]
	public async Task RefreshAccessTokenFromRefreshTokenAsync_ClientReturnsSuccess_ReturnSuccess()
	{
		// Arrange
		Mock<IResponse<RefreshAccessTokenResponse>> responseMock = new();

		responseMock
			.Setup(r => r.GetResult(It.IsAny<Func<RefreshAccessTokenResponse, string>>()))
			.Returns(Result.Success("token"));

		_hubSpotClientMock
			.Setup(client => client.PostAsync<RefreshAccessTokenResponse>(
				It.IsAny<RestRequest>(),
				It.IsAny<CancellationToken>()))
			.ReturnsAsync(responseMock.Object);

		// Act
		var result = await _uut.RefreshAccessTokenFromRefreshTokenAsync("", default);

		// Assert
		Assert.True(result.IsSuccess);
	}
}
