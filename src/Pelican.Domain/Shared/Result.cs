namespace Pelican.Domain.Shared;
public class Result
{
	protected internal Result(bool isSucces, Error error)
	{
		if (isSucces && error != Error.None) throw new InvalidOperationException();
		if (!isSucces && error == Error.None) throw new InvalidOperationException();

		IsSuccess = isSucces;
		Error = error;
	}


	public bool IsSuccess { get; }

	public bool IsFailure => !IsSuccess;

	public Error Error { get; }

	public static Result Success() => new(true, Error.None);

	public static Result<TValue> Success<TValue>(TValue value) => new(value, true, Error.None);

	public static Result Failure(Error error) => new(false, error);

	public static Result<TValue> Failure<TValue>(Error error) => new(default, false, error);

	public static Result<TValue> Create<TValue>(TValue value, Error error)
		where TValue : class
		=> value is not null
		? Success(value)
		: Failure<TValue>(error);

	public static Result FirstFailureOrSuccess(params Result[] results)
	{
		foreach (Result result in results)
		{
			if (result.IsFailure) return result;
		}
		return Success();
	}
}
