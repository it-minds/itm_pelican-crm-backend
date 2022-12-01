using RestSharp;

namespace Pelican.Application.Abstractions.Infrastructure;

public interface IClient<TSettings>
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
