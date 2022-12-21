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
	public void LoginommandValidator_NoEmptyStrings_ReturnsNoError()
	{
		// Arrange
		LoginCommand command = new(
			"notEmpty",
			"notEmpty");

		// Act
		TestValidationResult<LoginCommand> result = _uut.TestValidate(command);

		// Assert
		result.ShouldNotHaveValidationErrorFor(command => command.Email);
		result.ShouldNotHaveValidationErrorFor(command => command.Password);
	}
}
