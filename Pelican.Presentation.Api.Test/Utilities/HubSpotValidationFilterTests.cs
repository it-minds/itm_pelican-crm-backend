using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Pelican.Infrastructure.HubSpot.Settings;
using Pelican.Presentation.Api.Utilities;
using Xunit;

namespace Pelican.Presentation.Api.Test.Utilities;
public class HubSpotValidationFilterTests
{
	private readonly Mock<ILogger<HubSpotValidationFilter>> _loggerMock;
	private readonly Mock<IOptions<HubSpotSettings>> _optionsMock;
	private readonly HubSpotValidationFilter _uut;

	public HubSpotValidationFilterTests()
	{
		_loggerMock = new();
		_optionsMock = new();

		_uut = new(
			_optionsMock.Object,
			_loggerMock.Object);
	}

	[Fact]
	public void OnResourceExecuted_NoErrorsThrown()
	{
		/// Arrange
		ResourceExecutedContext resourceExecutedContext = new(
			new Microsoft.AspNetCore.Mvc.ActionContext(),
			new List<IFilterMetadata>());

		/// Act
		var exception = Record.Exception(() => _uut.OnResourceExecuted(resourceExecutedContext));

		/// Assert
		Assert.Null(exception);
	}
}
