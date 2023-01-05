using FluentValidation.TestHelper;
using Pelican.Application.Users.Commands.UpdatePassword;
using Pelican.Domain;
using Xunit;

namespace Pelican.Application.Test.Users.Commands.CreateStandardUser;
public class UpdatePasswordCommandValidatorTests
{
	private readonly UpdatePasswordCommandValidator _uut = new();

	[Theory]
	[InlineData("", "Password cannot be empty.")]
	[InlineData("text", "Password length must be a minimum of 12 characters.")]
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
	public void UpdatePasswordCommandValidtor_PasswordTooLong_ReturnsError()
	{
		// Arrange
		UpdatePasswordCommand command = new(
			new string('s', StringLengths.Password * 2));

		// Act
		TestValidationResult<UpdatePasswordCommand> result = _uut.TestValidate(command);

		// Assert
		result
			.ShouldHaveValidationErrorFor(command => command.Password)
			.WithErrorMessage("Password cannot be longer than " + $"{StringLengths.Password}.");
	}

	[Theory]
	[InlineData("1NewLongPassword!")]
	[InlineData("*,.!1Lonasdbaisgiuhiuahsd")]
	[InlineData("_jS3NaxDE(C#dQz&J?")]
	[InlineData("8+++Q8!^n3YA20.@cNHr")]
	public void CreateStandardUserCommandValidator_NoEmptyStringsPasswordInCorrectFormat_ReturnsNoError(string validPassword)
	{
		// Arrange
		UpdatePasswordCommand command = new(
			validPassword);

		// Act
		TestValidationResult<UpdatePasswordCommand> result = _uut.TestValidate(command);

		// Assert
		result
			.ShouldNotHaveValidationErrorFor(command => command.Password);
	}
}
