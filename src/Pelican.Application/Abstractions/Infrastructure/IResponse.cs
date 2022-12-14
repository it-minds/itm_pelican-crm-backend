namespace Pelican.Application.Abstractions.Infrastructure;

public interface IResponse<TResponse>
{
	TResponse? Data { get; }

	Result<TResult> GetResult<TResult>(Func<TResponse, TResult> mappingFunc);

	Task<Result<TResult>> GetResultWithUnitOfWork<TResult>(
		Func<TResponse, IUnitOfWork, CancellationToken, Task<TResult>> mappingFunc,
		IUnitOfWork unitOfWork,
		CancellationToken cancellationToken);
}
