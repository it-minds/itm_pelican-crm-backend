using FluentValidation.TestHelper;
using Pelican.Application.Clients.HubSpotCommands.DeleteClient;
using Xunit;

namespace Pelican.Application.Test.Clients.Commands.DeleteClient;
public class DeleteClientHubSpotCommandValidatorUnitTest
{
	private readonly DeleteClientHubspotCommandValidator _uut;
	public DeleteClientHubSpotCommandValidatorUnitTest()
	{
		_uut = new();
	}

	[Fact]
	public void DeleteClientCommandValidator_EmptyId_ReturnsError()
	{
		// Arrange
		DeleteClientHubSpotCommand command = new(0);

		// Act
		TestValidationResult<DeleteClientHubSpotCommand> result = _uut.TestValidate(command);

		// Assert
		result.ShouldHaveValidationErrorFor(command => command.ObjectId);
	}

	[Theory]
	[InlineData(long.MaxValue)]
	[InlineData(long.MinValue)]
	public void DeleteClientCommandValidator_NoEmptyId_ReturnsNoError(long objecId)
	{
		// Arrange
		DeleteClientHubSpotCommand command = new(objecId);

		// Act
		TestValidationResult<DeleteClientHubSpotCommand> result = _uut.TestValidate(command);

		// Assert
		result.ShouldNotHaveValidationErrorFor(command => command.ObjectId);
	}
}
