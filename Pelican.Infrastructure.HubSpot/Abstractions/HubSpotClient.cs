﻿using Microsoft.Extensions.Options;
using Pelican.Infrastructure.HubSpot.Settings;
using RestSharp;

namespace Pelican.Infrastructure.HubSpot.Abstractions;

internal sealed class RestSharpHubSpotClient : IHubSpotClient
{
	private readonly RestClient _client;

	public RestSharpHubSpotClient(
		IOptions<HubSpotSettings> options)
	{
		if (options is null
			|| options.Value is null
			|| options.Value.BaseUrl is null or "")
		{
			throw new ArgumentNullException(nameof(options));
		}

		_client = new RestClient(options.Value.BaseUrl);
	}

	public async Task<RestResponse<TResponse>> GetAsync<TResponse>(
		RestRequest request,
		CancellationToken cancellationToken)
	{
		return await _client
			.ExecuteGetAsync<TResponse>(request, cancellationToken);
	}

	public async Task<RestResponse<TResponse>> PostAsync<TResponse>(
		RestRequest request,
		CancellationToken cancellationToken)
	{
		return await _client
			.ExecutePostAsync<TResponse>(request, cancellationToken);
	}
}
