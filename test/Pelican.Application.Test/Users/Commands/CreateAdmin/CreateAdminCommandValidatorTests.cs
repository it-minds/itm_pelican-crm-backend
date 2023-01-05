using FluentValidation.TestHelper;
using Pelican.Application.Users.Commands.CreateAdmin;
using Pelican.Domain;
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
		result
			.ShouldHaveValidationErrorFor(command => command.Name);
		result
			.ShouldHaveValidationErrorFor(command => command.Email);
		result
			.ShouldHaveValidationErrorFor(command => command.Password)
			.WithErrorMessage("Password cannot be empty.");
	}

	[Fact]
	public void CreateAdminCommandValidator_AllStringsTooLong_ReturnsError()
	{
		// Arrange
		CreateAdminCommand command = new(
			new string('s', StringLengths.Name * 2),
			new string('s', StringLengths.Email * 2),
			new string('s', StringLengths.Password * 2));

		// Act
		TestValidationResult<CreateAdminCommand> result = _uut.TestValidate(command);

		// Assert
		result
			.ShouldHaveValidationErrorFor(command => command.Name)
			.WithErrorMessage("Name cannot be longer than " + $"{StringLengths.Name}.");
		result
			.ShouldHaveValidationErrorFor(command => command.Email)
			.WithErrorMessage("Email cannot be longer than " + $"{StringLengths.Email}.");
		result
			.ShouldHaveValidationErrorFor(command => command.Password)
			.WithErrorMessage("Password cannot be longer than " + $"{StringLengths.Password}.");
	}

	[Theory]
	[InlineData("shortPW", "Password length must be a minimum of 12 characters.")]
	[InlineData("newlongPassword", "Password must contain at least one number.")]
	[InlineData("1newlongpassword", "Password must contain at least one uppercase letter.")]
	[InlineData("1NEWLONGPASSWORD", "Password must contain at least one lowercase letter.")]
	[InlineData("1NewLongPassword", "Password must contain one or more special characters.")]
	public void CreateAdminCommandValidator_NoEmptyStringsAndPasswordIsInvalidFormat_ReturnsError(string invalidPassword, string expectedErrorMessage)
	{
		// Arrange
		CreateAdminCommand command = new(
			"notEmpty",
			"a@a.com",
			invalidPassword);

		// Act
		TestValidationResult<CreateAdminCommand> result = _uut.TestValidate(command);

		// Assert
		result
			.ShouldNotHaveValidationErrorFor(command => command.Name);
		result
			.ShouldNotHaveValidationErrorFor(command => command.Email);
		result
			.ShouldHaveValidationErrorFor(command => command.Password)
			.WithErrorMessage(expectedErrorMessage);
	}

	[Theory]
	[InlineData("1NewLongPassword!")]
	[InlineData("*,.!1Lonasdbaisgiuhiuahsd")]
	[InlineData("_jS3NaxDE(C#dQz&J?")]
	[InlineData("8+++Q8!^n3YA20.@cNHr")]
	public void CreateAdminCommandValidator_NoEmptyStringsPasswordInCorrectFormat_ReturnsNoError(string validPassword)
	{
		// Arrange
		CreateAdminCommand command = new(
			"notEmpty",
			"a@a.com",
			validPassword);

		// Act
		TestValidationResult<CreateAdminCommand> result = _uut.TestValidate(command);

		// Assert
		result
			.ShouldNotHaveValidationErrorFor(command => command.Name);
		result
			.ShouldNotHaveValidationErrorFor(command => command.Email);
		result
			.ShouldNotHaveValidationErrorFor(command => command.Password);
	}
}
