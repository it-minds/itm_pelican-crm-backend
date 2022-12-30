using FluentValidation.TestHelper;
using Pelican.Application.Users.Commands.CreateStandardUser;
using Pelican.Application.Users.Commands.UpdatePassword;
using Xunit;

namespace Pelican.Application.Test.Users.Commands.CreateStandardUser;
public class UpdatePasswordCommandValidatorTests
{
	private readonly UpdatePasswordCommandValidator _uut = new();

	[Fact]
	public void UpdatePasswordCommandValidator_EmptyString_ReturnsError()
	{
		// Arrange
		UpdatePasswordCommand command = new(string.Empty);

		// Act
		var result = _uut.TestValidate(command);

		// Assert
		result.ShouldHaveValidationErrorFor(command => command.Password).WithErrorMessage("Your password cannot be empty");
	}

	[Fact]
	public void UpdatePasswordCommandValidator_NoEmptyStringsButEmailIsNotInCorrectFormatAndPasswordIsTooShort_ReturnsError()
	{
		// Arrange
		UpdatePasswordCommand command = new("text");

		// Act
		TestValidationResult<UpdatePasswordCommand> result = _uut.TestValidate(command);

		// Assert
		result.ShouldHaveValidationErrorFor(command => command.Password).WithErrorMessage("Your password length must be a minimum of 8 characters");
	}

	[Fact]
	public void UpdatePasswordCommandValidator_NoEmptyStringsAndEmailInCorrectFormatAndPasswordContainsNoNumber_ReturnsNoError()
	{
		// Arrange
		UpdatePasswordCommand command = new("notEmpty");

		// Act
		TestValidationResult<UpdatePasswordCommand> result = _uut.TestValidate(command);

		// Assert
		result.ShouldHaveValidationErrorFor(command => command.Password).WithErrorMessage("Your password must contain at least one number.");
	}

	[Fact]
	public void UpdatePasswordCommandValidator_NoEmptyStringsAndEmailInCorrectFormatAndPasswordContainsNoUpperCaseChar_ReturnsError()
	{
		// Arrange
		UpdatePasswordCommand command = new("1newpassword");

		// Act
		TestValidationResult<UpdatePasswordCommand> result = _uut.TestValidate(command);

		// Assert
		result.ShouldHaveValidationErrorFor(command => command.Password).WithErrorMessage("Your password must contain at least one uppercase letter.");
	}
	[Fact]
	public void UpdatePasswordCommandValidator_NoEmptyStringsAndEmailInCorrectFormatAndPasswordContainsNoLowerCaseChar_ReturnsError()
	{
		// Arrange
		UpdatePasswordCommand command = new("1NEWPASSWORD");

		// Act
		TestValidationResult<UpdatePasswordCommand> result = _uut.TestValidate(command);

		// Assert
		result.ShouldHaveValidationErrorFor(command => command.Password).WithErrorMessage("Your password must contain at least one lowercase letter.");
	}
	[Fact]
	public void UpdatePasswordCommandValidator_NoEmptyStringsAndEmailAndPasswordInCorrectFormat_ReturnsNoError()
	{
		// Arrange
		UpdatePasswordCommand command = new("1NewPassword");

		// Act
		TestValidationResult<UpdatePasswordCommand> result = _uut.TestValidate(command);

		// Assert
		result.ShouldNotHaveValidationErrorFor(command => command.Password);
	}
}
