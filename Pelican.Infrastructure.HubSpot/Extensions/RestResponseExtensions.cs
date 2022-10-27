using Pelican.Domain.Shared;
using RestSharp;

namespace Pelican.Infrastructure.HubSpot.Extensions;

internal static class RestResponseExtensions
{
	internal static Result<TResult> GetResult<TResponse, TResult>(
		this RestResponse<TResponse> response,
		Func<TResponse, TResult> mappingFunc)
	{
		if (response is null)
		{
			return Result.Failure<TResult>(Error.NullValue);
		}

		if (response.IsSuccessful && response.Data is not null)
		{
			try
			{
				return Result.Success(mappingFunc(response.Data));
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
					response.StatusCode.ToString(),
					response.ErrorException?.Message! ?? "Error while fetching"));
	}
}
