using FluentValidation.TestHelper;
using Pelican.Application.Users.Commands.UpdateUser;
using Pelican.Domain.Enums;
using Xunit;

namespace Pelican.Application.Test.Users.Commands.UpdateUser;

public class UpdateUserCommandValidatorTests
{
	private readonly UpdateUserCommandValidator _uut;

	public UpdateUserCommandValidatorTests()
	{
		_uut = new UpdateUserCommandValidator();
	}

	[Fact]
	public void UpdateDealCommandValidator_EmptyStringOrDefaultValue_ReturnsError()
	{
		// Arrange
		UpdateUserCommand command = new(new()
		{
			Id = Guid.Empty,
			Email = string.Empty,
			Name = string.Empty,
		});

		// Act
		var result = _uut.TestValidate(command);

		// Assert
		result.ShouldHaveValidationErrorFor(command => command.User.Id);
		result.ShouldHaveValidationErrorFor(command => command.User.Email);
		result.ShouldHaveValidationErrorFor(command => command.User.Name);
		result.ShouldHaveValidationErrorFor(command => command.User.Role);
	}

	[Fact]
	public void UpdateDealCommandValidator_NoEmptyStringsOrDefaultValues_ReturnsNoError()
	{
		// Arrange
		UpdateUserCommand command = new(new()
		{
			Id = Guid.NewGuid(),
			Email = "email@email.com",
			Name = "Nem Name",
			Role = RoleEnum.Standard,
		});

		// Act
		TestValidationResult<UpdateUserCommand> result = _uut.TestValidate(command);

		// Assert
		result.ShouldNotHaveValidationErrorFor(command => command.User.Id);
		result.ShouldNotHaveValidationErrorFor(command => command.User.Email);
		result.ShouldNotHaveValidationErrorFor(command => command.User.Name);
		result.ShouldNotHaveValidationErrorFor(command => command.User.Role);
	}
}
