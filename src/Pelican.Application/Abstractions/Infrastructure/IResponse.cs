using Pelican.Domain.Shared;

namespace Pelican.Application.Abstractions.Infrastructure;

public interface IResponse<TResponse>
{
	public Result<TResult> GetResult<TResult>(Func<TResponse, TResult> mappingFunc);
}
