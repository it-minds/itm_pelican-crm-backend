using System.Runtime.CompilerServices;
using RestSharp;

[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]
namespace Pelican.Infrastructure.HubSpot.Abstractions;

internal interface IHubSpotClient
{
	Task<RestResponse<TResponse>> GetAsync<TResponse>(
		RestRequest request,
		CancellationToken cancellationToken = default);

	Task<RestResponse<TResponse>> PostAsync<TResponse>(
		RestRequest request,
		CancellationToken cancellationToken = default);
}
