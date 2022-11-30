using FluentValidation.TestHelper;
using Pelican.Application.Contacts.Commands.UpdateContact;
using Pelican.Application.Deals.HubSpotCommands.UpdateDeal;
using Xunit;


namespace Pelican.Application.Test.Deals.Commands.UpdateDeal;

public class UpdateDealCommandValidatorTests
{
	private readonly UpdateDealHubSpotCommandValidator _uut;

	public UpdateDealCommandValidatorTests()
	{
		_uut = new UpdateDealHubSpotCommandValidator();
	}

	[Fact]
	public void UpdateDealCommandValidator_EmptyStringOrDefaultValue_ReturnsError()
	{
		// Arrange
		UpdateDealHubSpotCommand command = new(
			0,
			0,
			string.Empty,
			string.Empty);

		// Act
		TestValidationResult<UpdateDealHubSpotCommand> result = _uut.TestValidate(command);

		// Assert
		result.ShouldHaveValidationErrorFor(command => command.ObjectId);
		result.ShouldHaveValidationErrorFor(command => command.SupplierHubSpotId);
		result.ShouldHaveValidationErrorFor(command => command.PropertyName);
		result.ShouldHaveValidationErrorFor(command => command.PropertyValue);
	}

	[Fact]
	public void UpdateDealCommandValidator_NoEmptyStringsOrDefaultValues_ReturnsNoError()
	{
		// Arrange
		UpdateDealHubSpotCommand command = new(
			1,
			1,
			"notEmpty",
			"notEmpty");

		// Act
		TestValidationResult<UpdateDealHubSpotCommand> result = _uut.TestValidate(command);

		// Assert
		result.ShouldNotHaveValidationErrorFor(command => command.ObjectId);
		result.ShouldNotHaveValidationErrorFor(command => command.SupplierHubSpotId);
		result.ShouldNotHaveValidationErrorFor(command => command.PropertyName);
		result.ShouldNotHaveValidationErrorFor(command => command.PropertyValue);
	}

}
