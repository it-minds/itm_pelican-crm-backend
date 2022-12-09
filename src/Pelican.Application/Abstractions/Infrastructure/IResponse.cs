using Pelican.Application.Abstractions.Data.Repositories;
using Pelican.Domain.Shared;

namespace Pelican.Application.Abstractions.Infrastructure;

public interface IResponse<TResponse>
{
	Result<TResult> GetResultV1<TResult>(Func<TResponse, TResult> mappingFunc);

	Task<Result<TResult>> GetResult<TResult>(
		Func<TResponse, IUnitOfWork, CancellationToken, Task<TResult>> mappingFunc,
		IUnitOfWork unitOfWork,
		CancellationToken cancellationToken);
}
