using FluentValidation.TestHelper;
using Pelican.Application.Authentication.Login;
using Xunit;

namespace Pelican.Application.Test.Login.Commands;
public class LoginCommandValidatorTests
{
	private readonly LoginCommandValidator _uut = new();

	[Fact]
	public void LoginCommandValidator_EmptyString_ReturnsError()
	{
		// Arrange
		LoginCommand command = new(
			string.Empty,
			string.Empty);

		// Act
		TestValidationResult<LoginCommand> result = _uut.TestValidate(command);

		// Assert
		result.ShouldHaveValidationErrorFor(command => command.Email);
		result.ShouldHaveValidationErrorFor(command => command.Password);
	}

	[Fact]
	public void LoginCommandValidator_NoEmptyStringsButEmailAndPasswordIsNotInCorrectFormat_ReturnsError()
	{
		// Arrange
		LoginCommand command = new(
			"notEmpty",
			"notEmpty");

		// Act
		TestValidationResult<LoginCommand> result = _uut.TestValidate(command);

		// Assert
		result.ShouldHaveValidationErrorFor(command => command.Email);
		result.ShouldHaveValidationErrorFor(command => command.Password);
	}

	[Theory]
	[InlineData("shortPW", "Password length must be a minimum of 12 characters.")]
	[InlineData("newlongPassword", "Password must contain at least one number.")]
	[InlineData("1newlongpassword", "Password must contain at least one uppercase letter.")]
	[InlineData("1NEWLONGPASSWORD", "Password must contain at least one lowercase letter.")]
	[InlineData("1NewLongPassword", "Password must contain one or more special characters.")]
	public void LoginCommandValidator_EmailInValidFormatAndPasswordIsInvalidFormat_ReturnsError(string invalidPassword, string expectedErrorMessage)
	{
		// Arrange
		LoginCommand command = new(
			"a@a.com",
			invalidPassword);

		// Act
		TestValidationResult<LoginCommand> result = _uut.TestValidate(command);

		// Assert
		result.ShouldNotHaveValidationErrorFor(command => command.Email);
		result.ShouldHaveValidationErrorFor(command => command.Password).WithErrorMessage(expectedErrorMessage);
	}

	[Theory]
	[InlineData("1NewLongPassword!")]
	[InlineData("*,.!1Lonasdbaisgiuhiuahsd")]
	[InlineData("_jS3NaxDE(C#dQz&J?")]
	[InlineData("8+++Q8!^n3YA20.@cNHr")]
	public void LoginCommandValidator_PasswordAndEmailInCorrectFormat_ReturnsNoError(string validPassword)
	{
		// Arrange
		LoginCommand command = new(
			"a@a.com",
			validPassword);

		// Act
		TestValidationResult<LoginCommand> result = _uut.TestValidate(command);

		// Assert
		result.ShouldNotHaveValidationErrorFor(command => command.Email);
		result.ShouldNotHaveValidationErrorFor(command => command.Password);
	}
}
