using FluentValidation.TestHelper;
using Pelican.Application.AccountManagers.Commands.ValidateWebhookUserId;
using Xunit;


namespace Pelican.Application.Test.AccountManagers.Commands.ValidateWebhookUserId;

public class ValidateWebhookUserIdCommandValidatorTests
{
	private readonly ValidateWebhookUserIdCommandValidator _uut;

	public ValidateWebhookUserIdCommandValidatorTests()
	{
		_uut = new();
	}

	[Fact]
	public void UpdateDealCommandValidator_EmptyStringOrDefaultValue_ReturnsError()
	{
		// Arrange
		ValidateWebhookUserIdCommand command = new(0, 0);

		// Act
		TestValidationResult<ValidateWebhookUserIdCommand> result = _uut.TestValidate(command);

		// Assert
		result.ShouldHaveValidationErrorFor(command => command.UserId);
		result.ShouldHaveValidationErrorFor(command => command.SupplierHubSpotId);
	}

	[Fact]
	public void UpdateDealCommandValidator_NoEmptyStringsOrDefaultValues_ReturnsNoError()
	{
		// Arrange
		ValidateWebhookUserIdCommand command = new(1, 1);

		// Act
		TestValidationResult<ValidateWebhookUserIdCommand> result = _uut.TestValidate(command);

		// Assert
		result.ShouldNotHaveValidationErrorFor(command => command.UserId);
		result.ShouldNotHaveValidationErrorFor(command => command.SupplierHubSpotId);
	}

}
