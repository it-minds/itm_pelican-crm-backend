using FluentValidation.TestHelper;
using Pelican.Application.HubSpot.Commands.NewInstallation;
using Xunit;


namespace Pelican.Application.Test.HubSpot.Commands.NewInstallation;

public class NewInstallationCommandValidatorTests
{
	private readonly NewInstallationCommandValidator _uut;

	public NewInstallationCommandValidatorTests()
	{
		_uut = new NewInstallationCommandValidator();
	}

	[Fact]
	public void NewInstallationCommandValidator_EmptyCode_ReturnsError()
	{
		// Arrange
		NewInstallationCommand command = new(String.Empty);

		// Act
		TestValidationResult<NewInstallationCommand> result = _uut.TestValidate(command);

		// Assert
		result.ShouldHaveValidationErrorFor(command => command.Code);
	}

	[Fact]
	public void NewInstallationCommandValidator_NullCode_ReturnsError()
	{
		// Arrange
		NewInstallationCommand command = new("");

		// Act
		TestValidationResult<NewInstallationCommand> result = _uut.TestValidate(command);

		// Assert
		result.ShouldNotHaveValidationErrorFor(command => command.Code);
	}
}
