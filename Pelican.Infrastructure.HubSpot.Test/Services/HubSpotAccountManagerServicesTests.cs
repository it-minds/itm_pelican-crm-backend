﻿using Moq;
using Pelican.Infrastructure.HubSpot.Abstractions;
using Pelican.Infrastructure.HubSpot.Contracts.Responses.AccountManagers;
using Pelican.Infrastructure.HubSpot.Services;
using RestSharp;
using Xunit;

namespace Pelican.Infrastructure.HubSpot.Test.Services;

public class HubSpotAccountManagerServicesTests
{
	private const string ID = "Id";

	private readonly Mock<IHubSpotClient> _hubSpotClientMock;
	private readonly HubSpotAccountManagerService _uut;
	private readonly CancellationToken _cancellationToken;

	public HubSpotAccountManagerServicesTests()
	{
		_hubSpotClientMock = new();
		_cancellationToken = new();
		_uut = new HubSpotAccountManagerService(_hubSpotClientMock.Object);
	}

	[Fact]
	public async Task GetByIdAsync_ClientReturnsFailure_ReturnFailure()
	{
		/// Arrange
		_hubSpotClientMock
			.Setup(client => client.GetAsync<OwnerResponse>(
				It.IsAny<RestRequest>(),
				_cancellationToken))
			.ReturnsAsync(new RestResponse<OwnerResponse>()
			{
				IsSuccessStatusCode = false,
			});

		/// Act
		var result = await _uut.GetByIdAsync("", 0, _cancellationToken);

		/// Assert
		Assert.True(result.IsFailure);
	}

	[Fact]
	public async Task GetByIdAsync_ClientReturnsSuccess_ReturnSuccess()
	{
		/// Arrange
		OwnerResponse response = new()
		{
			Id = ID,
		};

		_hubSpotClientMock
			.Setup(client => client.GetAsync<OwnerResponse>(
				It.IsAny<RestRequest>(),
				_cancellationToken))
			.ReturnsAsync(new RestResponse<OwnerResponse>()
			{
				IsSuccessStatusCode = true,
				ResponseStatus = ResponseStatus.Completed,
				Data = response
			});

		/// Act
		var result = await _uut.GetByIdAsync("", 0, _cancellationToken);

		/// Assert
		Assert.True(result.IsSuccess);
		Assert.Equal("Id", result.Value.HubSpotId);
	}

	[Fact]
	public async Task GetAsync_ClientReturnsFailure_ReturnFailure()
	{
		/// Arrange
		_hubSpotClientMock
			.Setup(client => client.GetAsync<OwnersResponse>(
				It.IsAny<RestRequest>(),
				_cancellationToken))
			.ReturnsAsync(new RestResponse<OwnersResponse>()
			{
				IsSuccessStatusCode = false,
			});

		/// Act
		var result = await _uut.GetAsync("", _cancellationToken);

		/// Assert
		Assert.True(result.IsFailure);
	}

	[Fact]
	public async Task GetAsync_ClientReturnsSuccess_ReturnSuccess()
	{
		/// Arrange
		OwnersResponse responses = new()
		{
			Results = new OwnerResponse[]
			{
					new OwnerResponse()
					{
						Id = ID,
					},
			},
		};

		_hubSpotClientMock
			.Setup(client => client.GetAsync<OwnersResponse>(
				It.IsAny<RestRequest>(),
				_cancellationToken))
			.ReturnsAsync(new RestResponse<OwnersResponse>()
			{
				IsSuccessStatusCode = true,
				ResponseStatus = ResponseStatus.Completed,
				Data = responses
			});

		/// Act
		var result = await _uut.GetAsync("", _cancellationToken);

		/// Assert
		Assert.True(result.IsSuccess);
		Assert.Equal("Id", result.Value.First().HubSpotId);
	}
}