using Pelican.Domain.Primitives;
using Pelican.Domain.Shared;
using RestSharp;

namespace Pelican.Application.Abstractions.Infrastructure;

public class RestSharpResponse<TResponse> : RestResponse<TResponse>, IResponse<TResponse> where TResponse : class
{
	public Result<TResult> GetResult<TResult>(
		Func<TResponse, TResult> mappingFunc)
	{
		if (IsSuccessful && Data is not null)
		{
			try
			{
				return Result.Success(mappingFunc(Data));
			}
			catch (Exception ex)
			{
				return Result.Failure<TResult>(new Error(
					"MappingError",
					ex.Message));
			}
		}

		return Result.Failure<TResult>(
				new Error(
					StatusCode.ToString(),
					ErrorException?.Message! ?? "Error while fetching"));
	}
}
