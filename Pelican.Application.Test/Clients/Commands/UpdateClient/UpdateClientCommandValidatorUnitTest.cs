using FluentValidation.TestHelper;
using Pelican.Application.Clients.Commands.UpdateClient;
using Xunit;

namespace Pelican.Application.Test.Clients.Commands.UpdateClient;
public class UpdateClientCommandValidatorUnitTest
{
	private UpdateClientCommandValidator _uut;
	public UpdateClientCommandValidatorUnitTest()
	{
		_uut = new();
	}
	[Fact]
	public void UpdateClientCommandCommandValidator_EmptyOrDefaultInputs_ReturnsError()
	{
		// Arrange
		UpdateClientCommand command = new(
			0,
			0,
			string.Empty,
			string.Empty);

		// Act

		TestValidationResult<UpdateClientCommand> result = _uut.TestValidate(command);

		// Assert
		result.ShouldHaveValidationErrorFor(command => command.ObjectId);
		result.ShouldHaveValidationErrorFor(command => command.PortalId);
		result.ShouldHaveValidationErrorFor(command => command.PropertyName);
		result.ShouldHaveValidationErrorFor(command => command.PropertyValue);
	}

	[Fact]
	public void UpdateClientCommandCommandValidator_NoEmptyOrDefaultInput_ReturnsNoError()
	{
		// Arrange
		UpdateClientCommand command = new(
			1,
			1,
			"notEmpty",
			"notEmpty");

		// Act
		TestValidationResult<UpdateClientCommand> result = _uut.TestValidate(command);

		// Assert
		result.ShouldNotHaveValidationErrorFor(command => command.ObjectId);
		result.ShouldNotHaveValidationErrorFor(command => command.PortalId);
		result.ShouldNotHaveValidationErrorFor(command => command.PropertyName);
		result.ShouldNotHaveValidationErrorFor(command => command.PropertyValue);
	}
}
