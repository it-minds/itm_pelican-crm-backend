﻿namespace Pelican.Domain.Shared;
public class Result
{
	protected internal Result(bool isSucces, Error error)
	{
		if (isSucces && error != Error.None) throw new InvalidOperationException();
		if (!isSucces && error == Error.None) throw new InvalidOperationException();

		IsSucces = isSucces;
		Error = error;
	}

	
	public bool IsSucces { get; }
	
	public bool IsFailure => !IsSucces;
	
	public Error Error { get; }
	
	public static Result Success() => new(true, Error.None);
	
	public static Result<TValue> Success<TValue>(TValue value) => new(value, true, Error.None);
	
	public static Result Failure(Error error) => new(false, error);
	
	public static Result<TValue> Failure<TValue>(Error error) => new(default, false, error);
	
	public static Result<TValue> Create<TValue>(TValue? value) =>
		value is not null
		? Success(value)
		: Failure<TValue>(Error.NullValue);
}
