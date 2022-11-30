using System.Runtime.CompilerServices;
using Pelican.Domain.Primitives;
using RestSharp;

[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]
namespace Pelican.Application.Abstractions.Infrastructure;

public interface IClient
{
	Task<IResponse<TResponse>> GetAsync<TResponse>(
		RestRequest request,
		CancellationToken cancellationToken = default)
		where TResponse : class;

	Task<IResponse<TResponse>> PostAsync<TResponse>(
		RestRequest request,
		CancellationToken cancellationToken = default)
		where TResponse : class;

}
