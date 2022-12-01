using Azure;
using Pelican.Application.Abstractions.Infrastructure;
using Pelican.Domain.Primitives;
using Pelican.Domain.Shared;
using RestSharp;

namespace Pelican.Application.RestSharp;

public class RestSharpResponse<TResponse> : RestResponse<TResponse>, IResponse<TResponse> where TResponse : class
{
	public RestSharpResponse() { }

	public RestSharpResponse(RestResponse<TResponse> response)
	{
		Content = response.Content;
		RawBytes = response.RawBytes;
		ContentEncoding = response.ContentEncoding;
		ContentLength = response.ContentLength;
		ContentType = response.ContentType;
		Cookies = response.Cookies;
		ErrorMessage = response.ErrorMessage;
		ErrorException = response.ErrorException;
		Headers = response.Headers;
		ContentHeaders = response.ContentHeaders;
		IsSuccessStatusCode = response.IsSuccessStatusCode;
		ResponseStatus = response.ResponseStatus;
		ResponseUri = response.ResponseUri;
		Server = response.Server;
		StatusCode = response.StatusCode;
		StatusDescription = response.StatusDescription;
		Request = response.Request;
		RootElement = response.RootElement;
		Data = response.Data;
	}

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
