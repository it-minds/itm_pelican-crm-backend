using Microsoft.Extensions.Options;
using Pelican.Application.Abstractions.Infrastructure;
using Pelican.Domain.Settings;
using RestSharp;

namespace Pelican.Application.RestSharp;

public sealed class RestSharpClient<TSettings> : IClient<TSettings> where TSettings : BaseSettings
{
	private readonly RestClient _client;

	public RestSharpClient(
		IOptions<TSettings> options)
		=> _client = new RestClient(options.Value.BaseUrl);

	public async Task<IResponse<TResponse>> GetAsync<TResponse>(
		RestRequest request,
		CancellationToken cancellationToken = default)
		where TResponse : class
		=> await _client.ExecuteGetAsync<TResponse>(request, cancellationToken) as RestSharpResponse<TResponse>
			?? new RestSharpResponse<TResponse>() { IsSuccessStatusCode = false };

	public async Task<IResponse<TResponse>> PostAsync<TResponse>(
		RestRequest request,
		CancellationToken cancellationToken = default)
		where TResponse : class
		=> (RestSharpResponse<TResponse>)await _client.ExecutePostAsync<TResponse>(request, cancellationToken)
			?? new RestSharpResponse<TResponse>() { IsSuccessStatusCode = false };
}

