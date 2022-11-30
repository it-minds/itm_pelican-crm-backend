using Microsoft.Extensions.Options;
using Pelican.Application.Abstractions.Infrastructure;
using Pelican.Domain.Settings;
using RestSharp;

namespace Pelican.Application.Abstractions.Infrastructure;

public sealed class RestSharpClient : IClient
{
	private readonly RestClient _client;

	public RestSharpClient(
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
