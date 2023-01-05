using FluentValidation.TestHelper;
using Pelican.Application.Authentication.Login;
using Pelican.Domain;
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
	public void LoginCommandValidator_NoEmptyStringsButEmailNotInValidFormat_ReturnsError()
	{
		// Arrange
		LoginCommand command = new(
			"notEmpty",
			"notEmpty");

		// Act
		TestValidationResult<LoginCommand> result = _uut.TestValidate(command);

		// Assert
		result.ShouldHaveValidationErrorFor(command => command.Email);
		result.ShouldNotHaveValidationErrorFor(command => command.Password);
	}

	[Fact]
	public void LoginCommandValidator_EmailStringTooLong_ReturnsError()
	{
		// Arrange
		LoginCommand command = new(
			new string('s', StringLengths.Email * 2),
			new string("notEmpty"));

		// Act
		TestValidationResult<LoginCommand> result = _uut.TestValidate(command);

		// Assert
		result.ShouldHaveValidationErrorFor(command => command.Email).WithErrorMessage("Email cannot be longer than " + $"{StringLengths.Email}.");
		result.ShouldNotHaveValidationErrorFor(command => command.Password);
	}

	[Fact]
	public void LoginCommandValidator_NoEmptyStringsEmailAndPasswordInValidFormat_ReturnsSuccess()
	{
		// Arrange
		LoginCommand command = new(
			"a@a.com",
			"notEmpty");

		// Act
		TestValidationResult<LoginCommand> result = _uut.TestValidate(command);

		// Assert
		result.ShouldNotHaveValidationErrorFor(command => command.Email);
		result.ShouldNotHaveValidationErrorFor(command => command.Password);
	}
}
