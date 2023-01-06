using FluentValidation.TestHelper;
using Pelican.Application.Mails.SendResetPassword;
using Pelican.Domain;
using Xunit;

namespace Pelican.Application.Test.Mails.SendResetPasswordMailCommand;
public class SendResetPasswordCommandValidatorUnitTest
{
	private readonly SendResetPasswordCommandValidator _uut = new();
	[Fact]
	public void SendResetPasswordCommandValidator_EmptyString_ReturnsError()
	{
		// Arrange
		SendResetPasswordCommand command = new(
			string.Empty);

		// Act
		TestValidationResult<SendResetPasswordCommand> result = _uut.TestValidate(command);

		// Assert
		result.ShouldHaveValidationErrorFor(command => command.Email);
	}

	[Fact]
	public void SendResetPasswordCommandValidator_EmailNotInValidFormat_ReturnsError()
	{
		// Arrange
		SendResetPasswordCommand command = new(
			"notEmpty");

		// Act
		TestValidationResult<SendResetPasswordCommand> result = _uut.TestValidate(command);

		// Assert
		result.ShouldHaveValidationErrorFor(command => command.Email);
	}

	[Fact]
	public void SendResetPasswordCommandValidator_EmailStringTooLong_ReturnsError()
	{
		// Arrange
		SendResetPasswordCommand command = new(
			new string('s', StringLengths.Email * 2));

		// Act
		TestValidationResult<SendResetPasswordCommand> result = _uut.TestValidate(command);

		// Assert
		result.ShouldHaveValidationErrorFor(command => command.Email).WithErrorMessage("Email cannot be longer than " + $"{StringLengths.Email}.");
	}

	[Fact]
	public void SendResetPasswordValidator_EmailInValidFormat_ReturnsSuccess()
	{
		// Arrange
		SendResetPasswordCommand command = new(
			"a@a.com");

		// Act
		TestValidationResult<SendResetPasswordCommand> result = _uut.TestValidate(command);

		// Assert
		result.ShouldNotHaveValidationErrorFor(command => command.Email);
	}
}
