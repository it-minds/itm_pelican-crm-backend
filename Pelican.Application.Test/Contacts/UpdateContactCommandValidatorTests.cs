using FluentValidation.TestHelper;
using Pelican.Application.Contacts.Commands.UpdateContact;
using Xunit;

namespace Pelican.Application.Test.Contacts;

public class UpdateContactCommandValidatorTests
{
	private readonly UpdateContactCommandValidator _uut;

	public UpdateContactCommandValidatorTests()
	{
		_uut = new();
	}

	[Fact]
	public void UpdateContactommandValidator_EmptyStringOrDefaultValue_ReturnsError()
	{
		// Arrange
		UpdateContactCommand command = new(
			0,
			0,
			string.Empty,
			string.Empty);

		// Act
		TestValidationResult<UpdateContactCommand> result = _uut.TestValidate(command);

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
		UpdateContactCommand command = new(
			1,
			1,
			"notEmpty",
			"notEmpty");

		// Act
		TestValidationResult<UpdateContactCommand> result = _uut.TestValidate(command);

		// Assert
		result.ShouldNotHaveValidationErrorFor(command => command.ObjectId);
		result.ShouldNotHaveValidationErrorFor(command => command.SupplierHubSpotId);
		result.ShouldNotHaveValidationErrorFor(command => command.PropertyName);
		result.ShouldNotHaveValidationErrorFor(command => command.PropertyValue);
	}

}
