using HotChocolate.Types;
using Moq;
using Pelican.Application.Abstractions.Data.Repositories;
using Pelican.Application.RestSharp;
using RestSharp;
using Xunit;

namespace Pelican.Application.Test.RestSharp;

public class RestSharpResponseTests
{
	private readonly RestSharpResponse<string> _uut;
	private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();

	public RestSharpResponseTests()
	{
		_uut = new();
	}

	[Fact]
	public void RestSharpResponse_d()
	{
		// Arrange
		RestResponse<string> input = new() { IsSuccessStatusCode = true };

		// Act
		RestResponse<string> restResponse = new RestSharpResponse<string>(input);

		// Assert
		Assert.Equal(input.Content, restResponse.Content);
		Assert.Equal(input.RawBytes, restResponse.RawBytes);
		Assert.Equal(input.ContentEncoding, restResponse.ContentEncoding);
		Assert.Equal(input.ContentLength, restResponse.ContentLength);
		Assert.Equal(input.ContentType, restResponse.ContentType);
		Assert.Equal(input.Cookies, restResponse.Cookies);
		Assert.Equal(input.ErrorMessage, restResponse.ErrorMessage);
		Assert.Equal(input.ErrorException, restResponse.ErrorException);
		Assert.Equal(input.Headers, restResponse.Headers);
		Assert.Equal(input.ContentHeaders, restResponse.ContentHeaders);
		Assert.Equal(input.IsSuccessStatusCode, restResponse.IsSuccessStatusCode);
		Assert.Equal(input.IsSuccessful, restResponse.IsSuccessful);
		Assert.Equal(input.ResponseStatus, restResponse.ResponseStatus);
		Assert.Equal(input.ResponseUri, restResponse.ResponseUri);
		Assert.Equal(input.Server, restResponse.Server);
		Assert.Equal(input.StatusCode, restResponse.StatusCode);
		Assert.Equal(input.StatusDescription, restResponse.StatusDescription);
		Assert.Equal(input.Request, restResponse.Request);
		Assert.Equal(input.RootElement, restResponse.RootElement);
		Assert.Equal(input.Data, restResponse.Data);
	}

	[Fact]
	public void GetResult_NotSuccessful_ReturnFailure()
	{
		// Arrange
		Func<string, string> func = x => x;

		_uut.IsSuccessStatusCode = false;
		_uut.StatusCode = System.Net.HttpStatusCode.InternalServerError;
		_uut.ErrorException = new("message");

		// Act
		var result = _uut.GetResultV1(func);

		// Assert
		Assert.True(result.IsFailure);

		Assert.Equal(_uut.StatusCode.ToString(), result.Error.Code);

		Assert.Equal(_uut.ErrorException.Message, result.Error.Message);
	}

	[Fact]
	public void GetResult_dataIsNull_ReturnFailure()
	{
		// Arrange
		Func<string, string> func = x => x;

		_uut.IsSuccessStatusCode = true;

		// Act
		var result = _uut.GetResultV1(func);

		// Assert
		Assert.True(result.IsFailure);

		Assert.Equal(_uut.StatusCode.ToString(), result.Error.Code);

		Assert.Equal("Error while fetching", result.Error.Message);
	}

	[Fact]
	public void GetResult_SuccessMappingData_ReturnSuccess()
	{
		// Arrange
		static string func(string x) => x;

		_uut.IsSuccessStatusCode = true;
		_uut.ResponseStatus = ResponseStatus.Completed;
		_uut.Data = "hello";

		// Act
		var result = _uut.GetResultV1(func);

		// Assert
		Assert.True(result.IsSuccess);

		Assert.Equal(_uut.Data, result.Value);
	}

	[Fact]
	public void GetResult_FailureMappingData_ReturnFailure()
	{
		// Arrange
		Func<string, string> func = x => throw new InvalidCastException("err");

		_uut.IsSuccessStatusCode = true;
		_uut.ResponseStatus = ResponseStatus.Completed;
		_uut.Data = "hello";

		// Act
		var result = _uut.GetResultV1(func);

		// Assert
		Assert.True(result.IsFailure);

		Assert.Equal("MappingError", result.Error.Code);

		Assert.Equal("err", result.Error.Message);
	}

	[Fact]
	public async Task GetResultWithUnitOfWork_NotSuccessful_ReturnFailureAsync()
	{
		// Arrange
		Task<string> func(string x, IUnitOfWork unitOfWork, CancellationToken cancellationToken) => Task.Run(() => x);

		_uut.IsSuccessStatusCode = false;
		_uut.StatusCode = System.Net.HttpStatusCode.InternalServerError;
		_uut.ErrorException = new("message");

		// Act
		var result = await _uut.GetResultWithUnitOfWork(
			func,
			_unitOfWorkMock.Object,
			default);

		// Assert
		Assert.True(result.IsFailure);

		Assert.Equal(_uut.StatusCode.ToString(), result.Error.Code);

		Assert.Equal(_uut.ErrorException.Message, result.Error.Message);
	}

	[Fact]
	public async Task GetResultWithUnitOfWork_dataIsNull_ReturnFailureAsync()
	{
		// Arrange
		Task<string> func(string x, IUnitOfWork unitOfWork, CancellationToken cancellationToken) => Task.Run(() => x);

		_uut.IsSuccessStatusCode = true;

		// Act
		var result = await _uut.GetResultWithUnitOfWork(
			func,
			_unitOfWorkMock.Object,
			default);

		// Assert
		Assert.True(result.IsFailure);

		Assert.Equal(_uut.StatusCode.ToString(), result.Error.Code);

		Assert.Equal("Error while fetching", result.Error.Message);
	}

	[Fact]
	public async Task GetResultWithUnitOfWork_FailureMappingData_ReturnFailureAsync()
	{
		// Arrange
		Task<string> func(string x, IUnitOfWork unitOfWork, CancellationToken cancellationToken) => throw new InvalidCastException("err");

		_uut.IsSuccessStatusCode = true;
		_uut.ResponseStatus = ResponseStatus.Completed;
		_uut.Data = "hello";

		// Act
		var result = await _uut.GetResultWithUnitOfWork(
			func,
			_unitOfWorkMock.Object,
			default);

		// Assert
		Assert.True(result.IsFailure);

		Assert.Equal("MappingError", result.Error.Code);

		Assert.Equal("err", result.Error.Message);
	}

	[Fact]
	public async Task GetResultWithUnitOfWork_SuccessMappingData_ReturnSuccessAsync()
	{
		// Arrange
		Task<string> func(string x, IUnitOfWork unitOfWork, CancellationToken cancellationToken) => Task.Run(() => x);

		_uut.IsSuccessStatusCode = true;
		_uut.ResponseStatus = ResponseStatus.Completed;
		_uut.Data = "hello";

		// Act
		var result = await _uut.GetResultWithUnitOfWork(
			func,
			_unitOfWorkMock.Object,
			default);

		// Assert
		Assert.True(result.IsSuccess);

		Assert.Equal(_uut.Data, result.Value);
	}
}
