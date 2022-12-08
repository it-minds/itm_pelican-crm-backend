using FluentValidation.TestHelper;
using Pelican.Application.Clients.HubSpotCommands.UpdateClient;
using Xunit;

namespace Pelican.Application.Test.Clients.Commands.UpdateClient;
public class UpdateClientHubSpotCommandValidatorUnitTest
{
	private readonly UpdateClientHubSpotCommandValidator _uut;
	public UpdateClientHubSpotCommandValidatorUnitTest()
	{
		_uut = new();
	}
	[Fact]
	public void UpdateClientCommandValidator_EmptyOrDefaultInputs_ReturnsError()
	{
		// Arrange
		UpdateClientHubSpotCommand command = new(
			0,
			0,
			0,
			string.Empty,
			string.Empty);

		// Act

		TestValidationResult<UpdateClientHubSpotCommand> result = _uut.TestValidate(command);

		// Assert
		result.ShouldHaveValidationErrorFor(command => command.ObjectId);
		result.ShouldHaveValidationErrorFor(command => command.PortalId);
		result.ShouldHaveValidationErrorFor(command => command.PropertyName);
		result.ShouldHaveValidationErrorFor(command => command.PropertyValue);
		result.ShouldHaveValidationErrorFor(command => command.UpdateTime);
	}

	[Fact]
	public void UpdateClientCommandValidator_NoEmptyNorDefaultInput_ReturnsNoError()
	{
		// Arrange
		UpdateClientHubSpotCommand command = new(
			1,
			1,
			1,
			"notEmpty",
			"notEmpty");

		// Act
		TestValidationResult<UpdateClientHubSpotCommand> result = _uut.TestValidate(command);

		// Assert
		result.ShouldNotHaveValidationErrorFor(command => command.ObjectId);
		result.ShouldNotHaveValidationErrorFor(command => command.PortalId);
		result.ShouldNotHaveValidationErrorFor(command => command.PropertyName);
		result.ShouldNotHaveValidationErrorFor(command => command.PropertyValue);
		result.ShouldNotHaveValidationErrorFor(command => command.UpdateTime);
	}
}
