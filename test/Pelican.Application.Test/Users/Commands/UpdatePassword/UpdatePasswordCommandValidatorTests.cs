using System;
using FluentValidation.TestHelper;
using Pelican.Application.Users.Commands.CreateStandardUser;
using Pelican.Application.Users.Commands.UpdatePassword;
using Xunit;

namespace Pelican.Application.Test.Users.Commands.CreateStandardUser;
public class UpdatePasswordCommandValidatorTests
{
	private readonly UpdatePasswordCommandValidator _uut = new();

	[Theory]
	[InlineData("", "Password cannot be empty")]
	[InlineData("text", "Password length must be a minimum of 12 characters")]
	[InlineData("notEmpty", "Password must contain at least one number.")]
	[InlineData("1newpassword", "Password must contain at least one uppercase letter.")]
	[InlineData("1NEWPASSWORD", "Password must contain at least one lowercase letter.")]
	[InlineData("1NewPassword", "Password must contain one or more special characters.")]
	public void UpdatePasswordCommandValidator_EmptyString_ReturnsError(string password, string error)
	{
		// Arrange
		UpdatePasswordCommand command = new(password);

		// Act
		var result = _uut.TestValidate(command);

		// Assert
		result
			.ShouldHaveValidationErrorFor(command => command.Password)
			.WithErrorMessage(error);
	}

	[Fact]
	public void UpdatePasswordCommandValidator_PasswordInCorrectFormat_ReturnsNoError()
	{
		// Arrange
		UpdatePasswordCommand command = new("1NewPassword!");

		// Act
		TestValidationResult<UpdatePasswordCommand> result = _uut.TestValidate(command);

		// Assert
		result.ShouldNotHaveValidationErrorFor(command => command.Password);
	}
}
