using FluentValidation.TestHelper;
using Pelican.Application.Users.Commands.CreateAdmin;
using Xunit;

namespace Pelican.Application.Test.Users.Commands.CreateAdmin;
public class CreateAdminCommandValidatorTests
{
	private readonly CreateAdminCommandValidator _uut = new();

	[Fact]
	public void CreateAdminCommandValidator_EmptyString_ReturnsError()
	{
		// Arrange
		CreateAdminCommand command = new(
			string.Empty,
			string.Empty,
			string.Empty);

		// Act
		TestValidationResult<CreateAdminCommand> result = _uut.TestValidate(command);

		// Assert
		result.ShouldHaveValidationErrorFor(command => command.Name);
		result.ShouldHaveValidationErrorFor(command => command.Email);
		result.ShouldHaveValidationErrorFor(command => command.Password).WithErrorMessage("Your password cannot be empty");

	}

	[Fact]
	public void CreateAdminCommandValidator_NoEmptyStringsButEmailIsNotInCorrectFormatAndPasswordIsTooShort_ReturnsError()
	{
		// Arrange
		CreateAdminCommand command = new(
			"notEmpty",
			"notEmpty",
			"text");

		// Act
		TestValidationResult<CreateAdminCommand> result = _uut.TestValidate(command);

		// Assert
		result.ShouldNotHaveValidationErrorFor(command => command.Name);
		result.ShouldHaveValidationErrorFor(command => command.Email);
		result.ShouldHaveValidationErrorFor(command => command.Password).WithErrorMessage("Your password length must be a minimum of 8 characters");
	}

	[Fact]
	public void CreateAdminCommandValidator_NoEmptyStringsAndEmailInCorrectFormatAndPasswordContainsNoNumber_ReturnsNoError()
	{
		// Arrange
		CreateAdminCommand command = new(
			"notEmpty",
			"a@a.com",
			"notEmpty");

		// Act
		TestValidationResult<CreateAdminCommand> result = _uut.TestValidate(command);

		// Assert
		result.ShouldNotHaveValidationErrorFor(command => command.Name);
		result.ShouldNotHaveValidationErrorFor(command => command.Email);
		result.ShouldHaveValidationErrorFor(command => command.Password).WithErrorMessage("Your password must contain at least one number.");
	}

	[Fact]
	public void CreateAdminCommandValidator_NoEmptyStringsAndEmailInCorrectFormatAndPasswordContainsNoUpperCaseChar_ReturnsError()
	{
		// Arrange
		CreateAdminCommand command = new(
			"notEmpty",
			"a@a.com",
			"1newpassword");

		// Act
		TestValidationResult<CreateAdminCommand> result = _uut.TestValidate(command);

		// Assert
		result.ShouldNotHaveValidationErrorFor(command => command.Name);
		result.ShouldNotHaveValidationErrorFor(command => command.Email);
		result.ShouldHaveValidationErrorFor(command => command.Password).WithErrorMessage("Your password must contain at least one uppercase letter.");
	}
	[Fact]
	public void CreateAdminCommandValidator_NoEmptyStringsAndEmailInCorrectFormatAndPasswordContainsNoLowerCaseChar_ReturnsError()
	{
		// Arrange
		CreateAdminCommand command = new(
			"notEmpty",
			"a@a.com",
			"1NEWPASSWORD");

		// Act
		TestValidationResult<CreateAdminCommand> result = _uut.TestValidate(command);

		// Assert
		result.ShouldNotHaveValidationErrorFor(command => command.Name);
		result.ShouldNotHaveValidationErrorFor(command => command.Email);
		result.ShouldHaveValidationErrorFor(command => command.Password).WithErrorMessage("Your password must contain at least one lowercase letter.");
	}
	[Fact]
	public void CreateAdminCommandValidator_NoEmptyStringsAndEmailAndPasswordInCorrectFormat_ReturnsNoError()
	{
		// Arrange
		CreateAdminCommand command = new(
			"notEmpty",
			"a@a.com",
			"1NewPassword");

		// Act
		TestValidationResult<CreateAdminCommand> result = _uut.TestValidate(command);

		// Assert
		result.ShouldNotHaveValidationErrorFor(command => command.Name);
		result.ShouldNotHaveValidationErrorFor(command => command.Email);
		result.ShouldNotHaveValidationErrorFor(command => command.Password);
	}
}
