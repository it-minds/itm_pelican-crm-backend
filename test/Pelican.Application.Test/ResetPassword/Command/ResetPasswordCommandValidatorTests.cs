using FluentValidation.TestHelper;
using Pelican.Application.Authentication.ResetPassword;
using Pelican.Domain;
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
		result.ShouldHaveValidationErrorFor(command => command.NewPassword).WithErrorMessage("New Password cannot be empty.");
	}

	[Fact]
	public void ResetPasswordCommandValidtor_PassWordTooLong_ReturnsError()
	{
		// Arrange
		ResetPasswordCommand command = new(
			"NotEmpty",
			new string('s', StringLengths.Password * 2));

		// Act
		TestValidationResult<ResetPasswordCommand> result = _uut.TestValidate(command);

		// Assert
		result.ShouldHaveValidationErrorFor(command => command.NewPassword).WithErrorMessage("New Password cannot be longer than " + $"{StringLengths.Password}.");
	}

	[Theory]
	[InlineData("shortPW", "New Password length must be a minimum of 12 characters.")]
	[InlineData("newlongPassword", "New Password must contain at least one number.")]
	[InlineData("1newlongpassword", "New Password must contain at least one uppercase letter.")]
	[InlineData("1NEWLONGPASSWORD", "New Password must contain at least one lowercase letter.")]
	[InlineData("1NewLongPassword", "New Password must contain one or more special characters.")]
	public void ResetPasswordCommandValidator_NoEmptyStringsAndPasswordIsInvalidFormat_ReturnsError(string invalidPassword, string expectedErrorMessage)
	{
		// Arrange
		ResetPasswordCommand command = new(
			"notEmpty",
			invalidPassword);

		// Act
		TestValidationResult<ResetPasswordCommand> result = _uut.TestValidate(command);

		// Assert
		result.ShouldNotHaveValidationErrorFor(command => command.SSOToken);
		result.ShouldHaveValidationErrorFor(command => command.NewPassword).WithErrorMessage(expectedErrorMessage);
	}

	[Theory]
	[InlineData("1NewLongPassword!")]
	[InlineData("*,.!1Lonasdbaisgiuhiuahsd")]
	[InlineData("_jS3NaxDE(C#dQz&J?")]
	[InlineData("8+++Q8!^n3YA20.@cNHr")]
	public void ResetPasswordCommandValidator_NoEmptyStringsPasswordInCorrectFormat_ReturnsNoError(string validPassword)
	{
		// Arrange
		ResetPasswordCommand command = new(
			"notEmpty",
			validPassword);

		// Act
		TestValidationResult<ResetPasswordCommand> result = _uut.TestValidate(command);

		// Assert
		result.ShouldNotHaveValidationErrorFor(command => command.SSOToken);
		result.ShouldNotHaveValidationErrorFor(command => command.NewPassword);
	}
}
