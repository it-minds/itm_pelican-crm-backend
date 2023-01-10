using FluentValidation.TestHelper;
using Pelican.Application.Users.Commands.UpdateMe;
using Pelican.Domain;
using Pelican.Domain.Enums;
using Xunit;

namespace Pelican.Application.Test.Users.Commands.UpdateMe;

public class UpdateMeCommandValidatorTests
{
	private readonly UpdateMeCommandValidator _uut;

	public UpdateMeCommandValidatorTests()
	{
		_uut = new UpdateMeCommandValidator();
	}

	[Fact]
	public void UpdateMeCommandValidator_EmptyStringOrDefaultValue_ReturnsError()
	{
		// Arrange
		UpdateMeCommand command = new(new()
		{
			Id = Guid.Empty,
			Email = string.Empty,
			Name = string.Empty,
		});

		// Act
		var result = _uut.TestValidate(command);

		// Assert
		result
			.ShouldHaveValidationErrorFor(command => command.User.Id);
		result
			.ShouldHaveValidationErrorFor(command => command.User.Email);
		result
			.ShouldHaveValidationErrorFor(command => command.User.Name);
		result
			.ShouldHaveValidationErrorFor(command => command.User.Role);
	}

	[Fact]
	public void UpdateMeCommandValidator_TooLongStringOrDefaultValue_ReturnsError()
	{
		// Arrange
		UpdateMeCommand command = new(new()
		{
			Id = Guid.Empty,
			Email = new string('x', StringLengths.Email * 2),
			Name = new string('x', StringLengths.Name * 2),
		});

		// Act
		var result = _uut.TestValidate(command);

		// Assert
		result
			.ShouldHaveValidationErrorFor(command => command.User.Id);
		result
			.ShouldHaveValidationErrorFor(command => command.User.Email)
			.WithErrorMessage("User Email cannot be longer than " + $"{StringLengths.Email}.");
		result
			.ShouldHaveValidationErrorFor(command => command.User.Name)
			.WithErrorMessage("User Name cannot be longer than " + $"{StringLengths.Name}.");
		result
			.ShouldHaveValidationErrorFor(command => command.User.Role);
	}

	[Fact]
	public void UpdateMeCommandValidator_EmailWrongFormat_ReturnsError()
	{
		// Arrange
		UpdateMeCommand command = new(new()
		{
			Id = Guid.NewGuid(),
			Email = "testWronglyFormattedEmail",
			Name = "testName",
			Role = RoleEnum.Admin,
		});

		// Act
		var result = _uut.TestValidate(command);

		// Assert
		result
			.ShouldNotHaveValidationErrorFor(command => command.User.Id);
		result
			.ShouldHaveValidationErrorFor(command => command.User.Email)
			.WithErrorMessage("User Email is not a valid email.");
		result
			.ShouldNotHaveValidationErrorFor(command => command.User.Name);
		result
			.ShouldNotHaveValidationErrorFor(command => command.User.Role);
	}

	[Fact]
	public void UpdateMeCommandValidator_NoEmptyStringsOrDefaultValues_ReturnsNoError()
	{
		// Arrange
		UpdateMeCommand command = new(new()
		{
			Id = Guid.NewGuid(),
			Email = "email@email.com",
			Name = "Nem Name",
			Role = RoleEnum.Standard,
		});

		// Act
		TestValidationResult<UpdateMeCommand> result = _uut.TestValidate(command);

		// Assert
		result
			.ShouldNotHaveValidationErrorFor(command => command.User.Id);
		result
			.ShouldNotHaveValidationErrorFor(command => command.User.Email);
		result
			.ShouldNotHaveValidationErrorFor(command => command.User.Name);
		result
			.ShouldNotHaveValidationErrorFor(command => command.User.Role);
	}

}
