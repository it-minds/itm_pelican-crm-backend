using FluentValidation.TestHelper;
using Pelican.Application.Contacts.HubSpotCommands.Update;
using Xunit;

namespace Pelican.Application.Test.Contacts.Commands.UpdateContact;

public class UpdateContactHubSpotCommandValidatorTests
{
	private readonly UpdateContactHubSpotCommandValidator _uut;

	public UpdateContactHubSpotCommandValidatorTests()
	{
		_uut = new();
	}

	[Fact]
	public void UpdateContactommandValidator_EmptyStringOrDefaultValue_ReturnsError()
	{
		// Arrange
		UpdateContactHubSpotCommand command = new(
			0,
			0,
			0,
			string.Empty,
			string.Empty);

		// Act
		TestValidationResult<UpdateContactHubSpotCommand> result = _uut.TestValidate(command);

		// Assert
		result.ShouldHaveValidationErrorFor(command => command.ObjectId);
		result.ShouldHaveValidationErrorFor(command => command.SupplierHubSpotId);
		result.ShouldHaveValidationErrorFor(command => command.PropertyName);
		result.ShouldHaveValidationErrorFor(command => command.PropertyValue);
	}

	[Fact]
	public void UpdateContactommandValidator_NoEmptyStringsOrDefaultValues_ReturnsNoError()
	{
		// Arrange
		UpdateContactHubSpotCommand command = new(
			1,
			1,
			1,
			"notEmpty",
			"notEmpty");

		// Act
		TestValidationResult<UpdateContactHubSpotCommand> result = _uut.TestValidate(command);

		// Assert
		result.ShouldNotHaveValidationErrorFor(command => command.ObjectId);
		result.ShouldNotHaveValidationErrorFor(command => command.SupplierHubSpotId);
		result.ShouldNotHaveValidationErrorFor(command => command.PropertyName);
		result.ShouldNotHaveValidationErrorFor(command => command.PropertyValue);
	}

}
