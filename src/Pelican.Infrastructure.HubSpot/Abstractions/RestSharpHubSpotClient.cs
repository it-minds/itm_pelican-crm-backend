using Microsoft.Extensions.Options;
using Pelican.Domain.Settings.HubSpot;
using RestSharp;

namespace Pelican.Infrastructure.HubSpot.Abstractions;

internal sealed class RestSharpHubSpotClient : IHubSpotClient
{
	private readonly RestClient _client;

	public RestSharpHubSpotClient(
		IOptions<HubSpotSettings> options)
	{
		_client = new RestClient(options.Value.BaseUrl);
	}

	public async Task<RestResponse<TResponse>> GetAsync<TResponse>(
		RestRequest request,
		CancellationToken cancellationToken = default)
		=> await _client.ExecuteGetAsync<TResponse>(request, cancellationToken);

	public async Task<RestResponse<TResponse>> PostAsync<TResponse>(
		RestRequest request,
		CancellationToken cancellationToken = default)
		=> await _client.ExecutePostAsync<TResponse>(request, cancellationToken);
}
