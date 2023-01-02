using FluentValidation.TestHelper;
using Pelican.Application.Users.Commands.CreateStandardUser;
using Xunit;

namespace Pelican.Application.Test.CreateStandardUser.Commands;
public class CreateStandardUserCommandValidatorTests
{
	private readonly CreateStandardUserCommandValidator _uut = new();

	[Fact]
	public void CreateStandardUserCommandValidator_EmptyString_ReturnsError()
	{
		// Arrange
		CreateStandardUserCommand command = new(
			string.Empty,
			string.Empty,
			string.Empty);

		// Act
		TestValidationResult<CreateStandardUserCommand> result = _uut.TestValidate(command);

		// Assert
		result.ShouldHaveValidationErrorFor(command => command.Name);
		result.ShouldHaveValidationErrorFor(command => command.Email);
		result.ShouldHaveValidationErrorFor(command => command.Password).WithErrorMessage("Your password cannot be empty");

	}

	[Fact]
	public void CreateStandardUserCommandValidator_NoEmptyStringsButEmailIsNotInCorrectFormatAndPasswordIsTooShort_ReturnsError()
	{
		// Arrange
		CreateStandardUserCommand command = new(
			"notEmpty",
			"notEmpty",
			"text");

		// Act
		TestValidationResult<CreateStandardUserCommand> result = _uut.TestValidate(command);

		// Assert
		result.ShouldNotHaveValidationErrorFor(command => command.Name);
		result.ShouldHaveValidationErrorFor(command => command.Email);
		result.ShouldHaveValidationErrorFor(command => command.Password).WithErrorMessage("Your password length must be a minimum of 8 characters");
	}

	[Fact]
	public void CreateStandardUserCommandValidator_NoEmptyStringsAndEmailInCorrectFormatAndPasswordContainsNoNumber_ReturnsNoError()
	{
		// Arrange
		CreateStandardUserCommand command = new(
			"notEmpty",
			"a@a.com",
			"notEmpty");

		// Act
		TestValidationResult<CreateStandardUserCommand> result = _uut.TestValidate(command);

		// Assert
		result.ShouldNotHaveValidationErrorFor(command => command.Name);
		result.ShouldNotHaveValidationErrorFor(command => command.Email);
		result.ShouldHaveValidationErrorFor(command => command.Password).WithErrorMessage("Your password must contain at least one number.");
	}

	[Fact]
	public void CreateStandardUserCommandValidator_NoEmptyStringsAndEmailInCorrectFormatAndPasswordContainsNoUpperCaseChar_ReturnsError()
	{
		// Arrange
		CreateStandardUserCommand command = new(
			"notEmpty",
			"a@a.com",
			"1newpassword");

		// Act
		TestValidationResult<CreateStandardUserCommand> result = _uut.TestValidate(command);

		// Assert
		result.ShouldNotHaveValidationErrorFor(command => command.Name);
		result.ShouldNotHaveValidationErrorFor(command => command.Email);
		result.ShouldHaveValidationErrorFor(command => command.Password).WithErrorMessage("Your password must contain at least one uppercase letter.");
	}
	[Fact]
	public void CreateStandardUserCommandValidator_NoEmptyStringsAndEmailInCorrectFormatAndPasswordContainsNoLowerCaseChar_ReturnsError()
	{
		// Arrange
		CreateStandardUserCommand command = new(
			"notEmpty",
			"a@a.com",
			"1NEWPASSWORD");

		// Act
		TestValidationResult<CreateStandardUserCommand> result = _uut.TestValidate(command);

		// Assert
		result.ShouldNotHaveValidationErrorFor(command => command.Name);
		result.ShouldNotHaveValidationErrorFor(command => command.Email);
		result.ShouldHaveValidationErrorFor(command => command.Password).WithErrorMessage("Your password must contain at least one lowercase letter.");
	}
	[Fact]
	public void CreateStandardUserCommandValidator_NoEmptyStringsAndEmailAndPasswordInCorrectFormat_ReturnsNoError()
	{
		// Arrange
		CreateStandardUserCommand command = new(
			"notEmpty",
			"a@a.com",
			"1NewPassword");

		// Act
		TestValidationResult<CreateStandardUserCommand> result = _uut.TestValidate(command);

		// Assert
		result.ShouldNotHaveValidationErrorFor(command => command.Name);
		result.ShouldNotHaveValidationErrorFor(command => command.Email);
		result.ShouldNotHaveValidationErrorFor(command => command.Password);
	}
}
