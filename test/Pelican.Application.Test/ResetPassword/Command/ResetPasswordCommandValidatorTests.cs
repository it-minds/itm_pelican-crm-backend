using FluentValidation.TestHelper;
using Pelican.Application.Authentication.ResetPassword;
using Xunit;

namespace Pelican.Application.Test.ResetPassword.Command;
public class ResetPasswordCommandValidatorTests
{
	private readonly ResetPasswordCommandValidator _uut = new();

	[Fact]
	public void ResetPasswordCommandValidator_EmptyString_ReturnsError()
	{
		// Arrange
		ResetPasswordCommand command = new(
			string.Empty,
			string.Empty);

		// Act
		TestValidationResult<ResetPasswordCommand> result = _uut.TestValidate(command);

		// Assert
		result.ShouldHaveValidationErrorFor(command => command.SSOToken);
		result.ShouldHaveValidationErrorFor(command => command.NewPassword).WithErrorMessage("Your password cannot be empty");

	}

	[Fact]
	public void ResetPasswordCommandValidator_NoEmptyStringsAndPasswordIsTooShort_ReturnsError()
	{
		// Arrange
		ResetPasswordCommand command = new(
			"notEmpty",
			"short");

		// Act
		TestValidationResult<ResetPasswordCommand> result = _uut.TestValidate(command);

		// Assert
		result.ShouldNotHaveValidationErrorFor(command => command.SSOToken);
		result.ShouldHaveValidationErrorFor(command => command.NewPassword).WithErrorMessage("Your password length must be a minimum of 8 characters");
	}

	[Fact]
	public void ResetPasswordCommandValidator_NoEmptyStringsAndPasswordContainsNoNumber_ReturnsNoError()
	{
		// Arrange
		ResetPasswordCommand command = new(
			"notEmpty",
			"newPassword");

		// Act
		TestValidationResult<ResetPasswordCommand> result = _uut.TestValidate(command);

		// Assert
		result.ShouldNotHaveValidationErrorFor(command => command.SSOToken);
		result.ShouldHaveValidationErrorFor(command => command.NewPassword).WithErrorMessage("Your password must contain at least one number.");
	}

	[Fact]
	public void ResetPasswordCommandValidator_NoEmptyStringsAndPasswordContainsNoUpperCaseChar_ReturnsError()
	{
		// Arrange
		ResetPasswordCommand command = new(
			"notEmpty",
			"1newpassword");

		// Act
		TestValidationResult<ResetPasswordCommand> result = _uut.TestValidate(command);

		// Assert
		result.ShouldNotHaveValidationErrorFor(command => command.SSOToken);
		result.ShouldHaveValidationErrorFor(command => command.NewPassword).WithErrorMessage("Your password must contain at least one uppercase letter.");
	}
	[Fact]
	public void ResetPasswordCommandValidator_NoEmptyStringsAndPasswordContainsNoLowerCaseChar_ReturnsError()
	{
		// Arrange
		ResetPasswordCommand command = new(
			"notEmpty",
			"1NEWPASSWORD");

		// Act
		TestValidationResult<ResetPasswordCommand> result = _uut.TestValidate(command);

		// Assert
		result.ShouldNotHaveValidationErrorFor(command => command.SSOToken);
		result.ShouldHaveValidationErrorFor(command => command.NewPassword).WithErrorMessage("Your password must contain at least one lowercase letter.");
	}
	[Fact]
	public void ResetPasswordCommandValidator_NoEmptyStringsPasswordInCorrectFormat_ReturnsNoError()
	{
		// Arrange
		ResetPasswordCommand command = new(
			"notEmpty",
			"1NewPassword");

		// Act
		TestValidationResult<ResetPasswordCommand> result = _uut.TestValidate(command);

		// Assert
		result.ShouldNotHaveValidationErrorFor(command => command.SSOToken);
		result.ShouldNotHaveValidationErrorFor(command => command.NewPassword);
	}
}
