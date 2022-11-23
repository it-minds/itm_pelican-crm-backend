using System.Linq.Expressions;
using Microsoft.Extensions.Options;
using Moq;
using Pelican.Application.Abstractions.Data.Repositories;
using Pelican.Domain.Entities;
using Pelican.Domain.Settings;
using Pelican.Domain.Shared;
using Pelican.Infrastructure.HubSpot.Abstractions;
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

	private readonly Mock<IHubSpotClient> _hubSpotClientMock;
	private readonly Mock<IOptions<HubSpotSettings>> _optionsMock;
	private readonly Mock<IUnitOfWork> _unitOfWorkMock;
	private readonly HubSpotAuthorizationService _uut;
	private readonly CancellationToken _cancellationToken;

	public HubSpotAuthorizationServicesTests()
	{
		_hubSpotClientMock = new();
		_optionsMock = new();
		_cancellationToken = new();
		_unitOfWorkMock = new();

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
		/// Arrange
		_hubSpotClientMock
			.Setup(client => client.PostAsync<GetAccessTokenResponse>(
				It.IsAny<RestRequest>(),
				_cancellationToken))
			.ReturnsAsync(new RestResponse<GetAccessTokenResponse>()
			{
				IsSuccessStatusCode = false,
			});

		/// Act
		var result = await _uut.AuthorizeUserAsync("", _cancellationToken);

		/// Assert
		Assert.True(result.IsFailure);
	}

	[Fact]
	public async Task AuthorizeUserAsync_ClientReturnsSuccess_ReturnSuccess()
	{
		/// Arrange
		GetAccessTokenResponse response = new()
		{
			RefreshToken = REFRESH,
			AccessToken = ACCESS,
		};

		_hubSpotClientMock
			.Setup(client => client.PostAsync<GetAccessTokenResponse>(
				It.IsAny<RestRequest>(),
				_cancellationToken))
			.ReturnsAsync(new RestResponse<GetAccessTokenResponse>()
			{
				IsSuccessStatusCode = true,
				ResponseStatus = ResponseStatus.Completed,
				Data = response
			});

		/// Act
		var result = await _uut.AuthorizeUserAsync("", _cancellationToken);

		/// Assert
		Assert.True(result.IsSuccess);
		Assert.Equal(REFRESH, result.Value.RefreshToken);
		Assert.Equal(ACCESS, result.Value.AccessToken);
	}

	[Fact]
	public async Task DecodeAccessTokenAsync_ClientReturnsFailure_ReturnFailure()
	{
		/// Arrange
		_hubSpotClientMock
			.Setup(client => client.PostAsync<AccessTokenResponse>(
				It.IsAny<RestRequest>(),
				_cancellationToken))
			.ReturnsAsync(new RestResponse<AccessTokenResponse>()
			{
				IsSuccessStatusCode = false,
			});

		/// Act
		var result = await _uut.DecodeAccessTokenAsync("", _cancellationToken);

		/// Assert
		Assert.True(result.IsFailure);
	}

	[Fact]
	public async Task DecodeAccessTokenAsync_ClientReturnsSuccess_ReturnSuccess()
	{
		const long ID = 123;
		const string DOMAIN = "domain";

		/// Arrange
		AccessTokenResponse response = new()
		{
			HubId = ID,
			HubDomain = DOMAIN,
		};

		_hubSpotClientMock
			.Setup(client => client.GetAsync<AccessTokenResponse>(
				It.IsAny<RestRequest>(),
				_cancellationToken))
			.ReturnsAsync(new RestResponse<AccessTokenResponse>()
			{
				IsSuccessStatusCode = true,
				ResponseStatus = ResponseStatus.Completed,
				Data = response
			});

		/// Act
		var result = await _uut.DecodeAccessTokenAsync("", _cancellationToken);

		/// Assert
		Assert.True(result.IsSuccess);
		Assert.Equal(ID, result.Value.HubSpotId);
		Assert.Equal(DOMAIN, result.Value.WebsiteUrl);
	}

	[Fact]
	public async Task RefreshAccessTokenAsync_SupplierDoesNotExist_ReturnFailure()
	{
		//Arrange
		_unitOfWorkMock.Setup(m => m.SupplierRepository.FindByCondition(It.IsAny<Expression<Func<Supplier, bool>>>()))
			.Returns(Enumerable.Empty<Supplier>().AsQueryable());
		//Act
		var result = await _uut.RefreshAccessTokenAsync(1, _unitOfWorkMock.Object, _cancellationToken);
		//Assert
		Assert.True(result.IsFailure);
		Assert.Equal(Error.NullValue, result.Error);

	}
	[Fact]
	public async Task RefreshAccessTokenAsync_SupplierRefreshTokenNull_ReturnFailure()
	{
		//Arrange
		Supplier supplier = new();
		_unitOfWorkMock.Setup(m => m.SupplierRepository.FindByCondition(It.IsAny<Expression<Func<Supplier, bool>>>()))
			.Returns(new List<Supplier> { supplier }.AsQueryable());

		/// Act
		var result = await _uut.RefreshAccessTokenAsync(1, _unitOfWorkMock.Object, _cancellationToken);

		/// Assert
		Assert.True(result.IsFailure);
		Assert.Equal(Error.NullValue, result.Error);
	}


	[Fact]
	public async Task RefreshAccessTokenAsync_ClientReturnsSuccess_ReturnSuccess()
	{
		/// Arrange
		Supplier supplier = new();
		supplier.RefreshToken = REFRESH;
		_unitOfWorkMock.Setup(m => m.SupplierRepository.FirstOrDefaultAsync(It.IsAny<Expression<Func<Supplier, bool>>>(), It.IsAny<CancellationToken>()))
			.ReturnsAsync(supplier);
		RefreshAccessTokenResponse response = new()
		{
			AccessToken = ACCESS,
		};

		_hubSpotClientMock
			.Setup(client => client.PostAsync<RefreshAccessTokenResponse>(
				It.IsAny<RestRequest>(),
				_cancellationToken))
			.ReturnsAsync(new RestResponse<RefreshAccessTokenResponse>()
			{
				IsSuccessStatusCode = true,
				ResponseStatus = ResponseStatus.Completed,
				Data = response
			});

		/// Act
		var result = await _uut.RefreshAccessTokenAsync(0, _unitOfWorkMock.Object, _cancellationToken);

		/// Assert
		Assert.True(result.IsSuccess);
		Assert.Equal(ACCESS, result.Value);
	}

}
