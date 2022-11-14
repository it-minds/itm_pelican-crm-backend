using Microsoft.Extensions.Options;
using Moq;
using Pelican.Domain.Settings;
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

	private readonly Mock<IHubSpotClient> _hubSpotClientMock;
	private readonly Mock<IOptions<HubSpotSettings>> _optionsMock;
	private readonly HubSpotAuthorizationService _uut;
	private readonly CancellationToken _cancellationToken;

	public HubSpotAuthorizationServicesTests()
	{
		_hubSpotClientMock = new();
		_optionsMock = new();
		_cancellationToken = new();

		_optionsMock
			.Setup(options => options.Value)
			.Returns(new HubSpotSettings()
			{
				RedirectUrl = REDIRECTURL,
				BaseUrl = BASEURL,
				App = new HubSpotAppSettings()
				{
					AppId = APPID,
					HubSpotClientId = CLIENTID,
					HubSpotClientSecret = CLIENTSECRET,
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
		const string REFRESH = "refresh";
		const string ACCESS = "access";

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
	public async Task RefreshAccessTokenAsync_ClientReturnsFailure_ReturnFailure()
	{
		/// Arrange
		_hubSpotClientMock
			.Setup(client => client.PostAsync<RefreshAccessTokenResponse>(
				It.IsAny<RestRequest>(),
				_cancellationToken))
			.ReturnsAsync(new RestResponse<RefreshAccessTokenResponse>()
			{
				IsSuccessStatusCode = false,
			});

		/// Act
		var result = await _uut.RefreshAccessTokenAsync("", _cancellationToken);

		/// Assert
		Assert.True(result.IsFailure);
	}


	[Fact]
	public async Task ARefreshAccessTokenAsync_ClientReturnsSuccess_ReturnSuccess()
	{
		const string ACCESS = "access";

		/// Arrange
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
		var result = await _uut.RefreshAccessTokenAsync("", _cancellationToken);

		/// Assert
		Assert.True(result.IsSuccess);
		Assert.Equal(ACCESS, result.Value);
	}

}
