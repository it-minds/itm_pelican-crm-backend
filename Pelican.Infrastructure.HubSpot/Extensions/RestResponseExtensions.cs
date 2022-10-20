using Pelican.Domain.Shared;
using RestSharp;

namespace Pelican.Infrastructure.HubSpot.Extensions;

internal static class RestResponseExtensions
{
	internal static Result<TResult> GetResult<TResponse, TResult>(
		this RestResponse<TResponse> response,
		Func<TResponse, TResult> mappingFunc)
	{
		if (response.IsSuccessful && response.Data is not null)
		{
			TResult result = mappingFunc(response.Data);

			return Result.Success(result);
		}

		return Result.Failure<TResult>(
				new Error(
					response.StatusCode.ToString(),
					response.ErrorException?.Message!));
	}
}
