namespace Pelican.Domain.Shared;

public class ResultDto<TValue>
{
	public bool IsSuccess { get; set; }
	public bool IsFailure => !IsSuccess;
	public Error? Error { get; set; }
	public TValue? Value { get; set; }

	public static implicit operator ResultDto<TValue>(Result<TValue> result) => new()
	{
		IsSuccess = result.IsSuccess,
		Error = result.Error,
		Value = result.IsSuccess ? result.Value : default,
	};
}
