using System.Runtime.CompilerServices;
using RestSharp;

[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]
namespace Pelican.Application.Abstractions.Infrastructure;

public interface IClient
{
	Task<RestResponse<TResponse>> GetAsync<TResponse>(
		RestRequest request,
		CancellationToken cancellationToken = default);

	Task<RestResponse<TResponse>> PostAsync<TResponse>(
		RestRequest request,
		CancellationToken cancellationToken = default);
}
