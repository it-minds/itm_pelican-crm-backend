using FluentValidation.TestHelper;
using Pelican.Application.Deals.Commands.UpdateDeal;
using Xunit;


namespace Pelican.Application.Test.Deals.Commands.UpdateDeal;

public class UpdateDealCommandValidatorTests
{
	private readonly UpdateDealCommandValidator _uut;

	public UpdateDealCommandValidatorTests()
	{
		_uut = new UpdateDealCommandValidator();
	}

	[Fact]
	public void UpdateDealCommandCommandValidator_EmptyString_ReturnsError()
	{
		// Arrange
		UpdateDealCommand command = new(
			0,
			0,
			string.Empty,
			string.Empty);

		// Act
		TestValidationResult<UpdateDealCommand> result = _uut.TestValidate(command);

		// Assert
		result.ShouldHaveValidationErrorFor(command => command.ObjectId);
		result.ShouldHaveValidationErrorFor(command => command.PortalId);
		result.ShouldHaveValidationErrorFor(command => command.PropertyName);
		result.ShouldHaveValidationErrorFor(command => command.PropertyValue);
	}

	[Fact]
	public void UpdateDealCommandCommandValidator_NoEmptyStrings_ReturnsNoError()
	{
		// Arrange
		UpdateDealCommand command = new(
			1,
			1,
			"notEmpty",
			"notEmpty");

		// Act
		TestValidationResult<UpdateDealCommand> result = _uut.TestValidate(command);

		// Assert
		result.ShouldNotHaveValidationErrorFor(command => command.ObjectId);
		result.ShouldNotHaveValidationErrorFor(command => command.PortalId);
		result.ShouldNotHaveValidationErrorFor(command => command.PropertyName);
		result.ShouldNotHaveValidationErrorFor(command => command.PropertyValue);
	}

}
