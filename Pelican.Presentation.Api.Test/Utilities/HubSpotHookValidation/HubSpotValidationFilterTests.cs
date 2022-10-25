using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using Moq;
using Pelican.Presentation.Api.Utilities.HubSpotHookValidation;
using Pelican.Presentation.Api.Utilities.HubSpotHookValidation.HashGenerator;
using Xunit;

namespace Pelican.Presentation.Api.Test.Utilities.HubSpotHookValidation;

public class HubSpotValidationFilterTests
{
	private readonly Mock<ILogger<HubSpotValidationFilter>> _loggerMock;
	private readonly Mock<IHashGeneratorFactory> _hashGeneratorFactoryMock;
	private readonly HubSpotValidationFilter _uut;

	private readonly Mock<IHashGenerator> _hashGeneratorMock;

	public HubSpotValidationFilterTests()
	{
		_loggerMock = new();
		_hashGeneratorFactoryMock = new();

		_uut = new(
			_hashGeneratorFactoryMock.Object,
			_loggerMock.Object);

		_hashGeneratorMock = new();
	}

	[Fact]
	public void OnResourceExecuted_NoErrorsThrown()
	{
		/// Arrange
		HttpContext httpContext = new DefaultHttpContext();
		RouteData routeData = new();
		ActionDescriptor actionDescriptor = new();

		ActionContext actionContext = new(
			httpContext,
			routeData,
			actionDescriptor);

		List<IFilterMetadata> filters = new();

		ResourceExecutedContext resourceExecutedContext = new(
			actionContext,
			filters);

		/// Act
		var exception = Record.Exception(() => _uut
			.OnResourceExecuted(resourceExecutedContext));

		/// Assert
		Assert.Null(exception);
	}

	[Fact]
	public void OnResourceExecuting_MissingSignatureAndSignatureVersion_ReturnsBadRequstObject()
	{
		/// Arrange
		HttpContext httpContext = new DefaultHttpContext();
		RouteData routeData = new();
		ActionDescriptor actionDescriptor = new();

		ActionContext actionContext = new(
			httpContext,
			routeData,
			actionDescriptor);

		List<IFilterMetadata> filters = new();

		List<IValueProviderFactory> valueProviderFactories = new();

		ResourceExecutingContext resourceExecutingContext = new(
			actionContext,
			filters,
			valueProviderFactories);


		/// Act
		var exception = Record.Exception(() => _uut
			.OnResourceExecuting(resourceExecutingContext));

		/// Assert
		Assert.Null(exception);

		_loggerMock.Verify(x =>
			x.Log(
				LogLevel.Warning,
				It.IsAny<EventId>(),
				It.Is<It.IsAnyType>((v, t) => true),
				It.IsAny<Exception>(),
				It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)),
			Times.Once());

		Assert.Equal(
			typeof(BadRequestObjectResult),
			resourceExecutingContext.Result?.GetType());

		Assert.Equal(
			"{ message = Unable to validate signature }",
			((BadRequestObjectResult)resourceExecutingContext.Result).Value.ToString());
	}

	[Fact]
	public void OnResourceExecuting_MissingSignature_ReturnsBadRequstObject()
	{
		/// Arrange
		HttpContext httpContext = new DefaultHttpContext();
		httpContext.Request.Headers.Add(
			"X-HubSpot-Signature-Version",
			"value");

		RouteData routeData = new();
		ActionDescriptor actionDescriptor = new();

		ActionContext actionContext = new(
			httpContext,
			routeData,
			actionDescriptor);

		List<IFilterMetadata> filters = new();

		List<IValueProviderFactory> valueProviderFactories = new();

		ResourceExecutingContext resourceExecutingContext = new(
			actionContext,
			filters,
			valueProviderFactories);

		/// Act
		var exception = Record.Exception(() => _uut
			.OnResourceExecuting(resourceExecutingContext));

		/// Assert
		Assert.Null(exception);

		_loggerMock.Verify(x =>
			x.Log(
				LogLevel.Warning,
				It.IsAny<EventId>(),
				It.Is<It.IsAnyType>((v, t) => true),
				It.IsAny<Exception>(),
				It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)),
			Times.Once());

		Assert.Equal(
			typeof(BadRequestObjectResult),
			resourceExecutingContext.Result?.GetType());

		Assert.Equal(
			"{ message = Unable to validate signature }",
			((BadRequestObjectResult)resourceExecutingContext.Result).Value.ToString());
	}

	[Fact]
	public void OnResourceExecuting_MissingSignatureVersion_ReturnsBadRequstObject()
	{
		/// Arrange
		HttpContext httpContext = new DefaultHttpContext();
		httpContext.Request.Headers.Add(
			"X-HubSpot-Signature",
			"value");

		RouteData routeData = new();
		ActionDescriptor actionDescriptor = new();

		ActionContext actionContext = new(
			httpContext,
			routeData,
			actionDescriptor);

		List<IFilterMetadata> filters = new();

		List<IValueProviderFactory> valueProviderFactories = new();

		ResourceExecutingContext resourceExecutingContext = new(
			actionContext,
			filters,
			valueProviderFactories);

		/// Act
		var exception = Record.Exception(() => _uut
			.OnResourceExecuting(resourceExecutingContext));

		/// Assert
		Assert.Null(exception);

		_loggerMock.Verify(x =>
			x.Log(
				LogLevel.Warning,
				It.IsAny<EventId>(),
				It.Is<It.IsAnyType>((v, t) => true),
				It.IsAny<Exception>(),
				It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)),
			Times.Once());

		Assert.Equal(
			typeof(BadRequestObjectResult),
			resourceExecutingContext.Result?.GetType());

		Assert.Equal(
			"{ message = Unable to validate signature }",
			((BadRequestObjectResult)resourceExecutingContext.Result).Value.ToString());
	}

	[Fact]
	public void OnResourceExecuting_UnsupportedVersion_ThrowsError()
	{
		/// Arrange
		HttpContext httpContext = new DefaultHttpContext();
		httpContext.Request.Headers.Add(
			"X-HubSpot-Signature",
			"value");
		httpContext.Request.Headers.Add(
			"X-HubSpot-Signature-Version",
			"v9");

		RouteData routeData = new();
		ActionDescriptor actionDescriptor = new();

		ActionContext actionContext = new(
			httpContext,
			routeData,
			actionDescriptor);

		List<IFilterMetadata> filters = new();

		List<IValueProviderFactory> valueProviderFactories = new();

		ResourceExecutingContext resourceExecutingContext = new(
			actionContext,
			filters,
			valueProviderFactories);

		_hashGeneratorFactoryMock
			.Setup(factory => factory.CreateHashGenerator(9))
			.Throws(new Exception("Unsupported signature version"));

		/// Act
		var exception = Record.Exception(() => _uut
			.OnResourceExecuting(resourceExecutingContext));

		/// Assert
		Assert.Equal(
			new Exception("Unsupported signature version").Message,
			exception.Message);
	}

	[Fact]
	public void OnResourceExecuting_Version1WrongHash_ThrowsError()
	{
		/// Arrange
		HttpContext httpContext = new DefaultHttpContext();
		httpContext.Request.Headers.Add(
			"X-HubSpot-Signature",
			"WrongHash");
		httpContext.Request.Headers.Add(
			"X-HubSpot-Signature-Version",
			"v1");

		RouteData routeData = new();
		ActionDescriptor actionDescriptor = new();

		ActionContext actionContext = new(
			httpContext,
			routeData,
			actionDescriptor);

		List<IFilterMetadata> filters = new();

		List<IValueProviderFactory> valueProviderFactories = new();

		ResourceExecutingContext resourceExecutingContext = new(
			actionContext,
			filters,
			valueProviderFactories);

		_hashGeneratorFactoryMock
			.Setup(factory => factory.CreateHashGenerator(1))
			.Returns(_hashGeneratorMock.Object);

		_hashGeneratorMock
			.Setup(hashGenerator => hashGenerator.GenerateHash(actionContext.HttpContext.Request))
			.Returns("NotEqualWrongHash");

		/// Act
		var exception = Record.Exception(() => _uut
			.OnResourceExecuting(resourceExecutingContext));

		/// Assert
		Assert.Null(exception);

		_loggerMock.Verify(x =>
			x.Log(
				LogLevel.Warning,
				It.IsAny<EventId>(),
				It.Is<It.IsAnyType>((v, t) => true),
				It.IsAny<Exception>(),
				It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)),
			Times.Once());

		Assert.Equal(
			typeof(BadRequestObjectResult),
			resourceExecutingContext.Result?.GetType());

		Assert.Equal(
			"{ message = Invalid request signature }",
			((BadRequestObjectResult)resourceExecutingContext.Result).Value.ToString());
	}

	[Fact]
	public void OnResourceExecuting_Version1CorrectHash_ThrowsError()
	{
		/// Arrange
		HttpContext httpContext = new DefaultHttpContext();
		httpContext.Request.Headers.Add(
			"X-HubSpot-Signature",
			"CorrectHash");
		httpContext.Request.Headers.Add(
			"X-HubSpot-Signature-Version",
			"v1");

		RouteData routeData = new();
		ActionDescriptor actionDescriptor = new();

		ActionContext actionContext = new(
			httpContext,
			routeData,
			actionDescriptor);

		List<IFilterMetadata> filters = new();

		List<IValueProviderFactory> valueProviderFactories = new();

		ResourceExecutingContext resourceExecutingContext = new(
			actionContext,
			filters,
			valueProviderFactories);


		_hashGeneratorFactoryMock
			.Setup(factory => factory.CreateHashGenerator(1))
			.Returns(_hashGeneratorMock.Object);

		_hashGeneratorMock
			.Setup(hashGenerator => hashGenerator.GenerateHash(actionContext.HttpContext.Request))
			.Returns("CorrectHash");

		/// Act
		var exception = Record.Exception(() => _uut
			.OnResourceExecuting(resourceExecutingContext));

		/// Assert
		Assert.Null(exception);
	}

	[Fact]
	public void OnResourceExecuting_Version2WrongHash_ThrowsError()
	{
		/// Arrange
		HttpContext httpContext = new DefaultHttpContext();
		httpContext.Request.Headers.Add(
			"X-HubSpot-Signature",
			"WrongHash");
		httpContext.Request.Headers.Add(
			"X-HubSpot-Signature-Version",
			"v2");

		RouteData routeData = new();
		ActionDescriptor actionDescriptor = new();

		ActionContext actionContext = new(
			httpContext,
			routeData,
			actionDescriptor);

		List<IFilterMetadata> filters = new();

		List<IValueProviderFactory> valueProviderFactories = new();

		ResourceExecutingContext resourceExecutingContext = new(
			actionContext,
			filters,
			valueProviderFactories);

		_hashGeneratorFactoryMock
			.Setup(factory => factory.CreateHashGenerator(2))
			.Returns(_hashGeneratorMock.Object);

		_hashGeneratorMock
			.Setup(hashGenerator => hashGenerator.GenerateHash(actionContext.HttpContext.Request))
			.Returns("NotEqualWrongHash");

		/// Act
		var exception = Record.Exception(() => _uut
			.OnResourceExecuting(resourceExecutingContext));

		/// Assert
		Assert.Null(exception);

		_loggerMock.Verify(x =>
			x.Log(
				LogLevel.Warning,
				It.IsAny<EventId>(),
				It.Is<It.IsAnyType>((v, t) => true),
				It.IsAny<Exception>(),
				It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)),
			Times.Once());

		Assert.Equal(
			typeof(BadRequestObjectResult),
			resourceExecutingContext.Result?.GetType());

		Assert.Equal(
			"{ message = Invalid request signature }",
			((BadRequestObjectResult)resourceExecutingContext.Result).Value.ToString());
	}

	[Fact]
	public void OnResourceExecuting_Version2CorrectHash_ThrowsError()
	{
		/// Arrange
		HttpContext httpContext = new DefaultHttpContext();
		httpContext.Request.Headers.Add(
			"X-HubSpot-Signature",
			"CorrectHash");
		httpContext.Request.Headers.Add(
			"X-HubSpot-Signature-Version",
			"v2");

		RouteData routeData = new();
		ActionDescriptor actionDescriptor = new();

		ActionContext actionContext = new(
			httpContext,
			routeData,
			actionDescriptor);

		List<IFilterMetadata> filters = new();

		List<IValueProviderFactory> valueProviderFactories = new();

		ResourceExecutingContext resourceExecutingContext = new(
			actionContext,
			filters,
			valueProviderFactories);


		_hashGeneratorFactoryMock
			.Setup(factory => factory.CreateHashGenerator(2))
			.Returns(_hashGeneratorMock.Object);

		_hashGeneratorMock
			.Setup(hashGenerator => hashGenerator.GenerateHash(actionContext.HttpContext.Request))
			.Returns("CorrectHash");

		/// Act
		var exception = Record.Exception(() => _uut
			.OnResourceExecuting(resourceExecutingContext));

		/// Assert
		Assert.Null(exception);
	}

	[Fact]
	public void OnResourceExecuting_Version3WrongHash_ThrowsError()
	{
		/// Arrange
		HttpContext httpContext = new DefaultHttpContext();
		httpContext.Request.Headers.Add(
			"X-HubSpot-Signature",
			"WrongHash");
		httpContext.Request.Headers.Add(
			"X-HubSpot-Signature-Version",
			"v3");

		RouteData routeData = new();
		ActionDescriptor actionDescriptor = new();

		ActionContext actionContext = new(
			httpContext,
			routeData,
			actionDescriptor);

		List<IFilterMetadata> filters = new();

		List<IValueProviderFactory> valueProviderFactories = new();

		ResourceExecutingContext resourceExecutingContext = new(
			actionContext,
			filters,
			valueProviderFactories);

		_hashGeneratorFactoryMock
			.Setup(factory => factory.CreateHashGenerator(3))
			.Returns(_hashGeneratorMock.Object);

		_hashGeneratorMock
			.Setup(hashGenerator => hashGenerator.GenerateHash(actionContext.HttpContext.Request))
			.Returns("NotEqualWrongHash");

		/// Act
		var exception = Record.Exception(() => _uut
			.OnResourceExecuting(resourceExecutingContext));

		/// Assert
		Assert.Null(exception);

		_loggerMock.Verify(x =>
			x.Log(
				LogLevel.Warning,
				It.IsAny<EventId>(),
				It.Is<It.IsAnyType>((v, t) => true),
				It.IsAny<Exception>(),
				It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)),
			Times.Once());

		Assert.Equal(
			typeof(BadRequestObjectResult),
			resourceExecutingContext.Result?.GetType());

		Assert.Equal(
			"{ message = Invalid request signature }",
			((BadRequestObjectResult)resourceExecutingContext.Result).Value.ToString());
	}

	[Fact]
	public void OnResourceExecuting_Version3CorrectHash_ThrowsError()
	{
		/// Arrange
		HttpContext httpContext = new DefaultHttpContext();
		httpContext.Request.Headers.Add(
			"X-HubSpot-Signature",
			"CorrectHash");
		httpContext.Request.Headers.Add(
			"X-HubSpot-Signature-Version",
			"v3");

		RouteData routeData = new();
		ActionDescriptor actionDescriptor = new();

		ActionContext actionContext = new(
			httpContext,
			routeData,
			actionDescriptor);

		List<IFilterMetadata> filters = new();

		List<IValueProviderFactory> valueProviderFactories = new();

		ResourceExecutingContext resourceExecutingContext = new(
			actionContext,
			filters,
			valueProviderFactories);


		_hashGeneratorFactoryMock
			.Setup(factory => factory.CreateHashGenerator(3))
			.Returns(_hashGeneratorMock.Object);

		_hashGeneratorMock
			.Setup(hashGenerator => hashGenerator.GenerateHash(actionContext.HttpContext.Request))
			.Returns("CorrectHash");

		/// Act
		var exception = Record.Exception(() => _uut
			.OnResourceExecuting(resourceExecutingContext));

		/// Assert
		Assert.Null(exception);
	}
}
