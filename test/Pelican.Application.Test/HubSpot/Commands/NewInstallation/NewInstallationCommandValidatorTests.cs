using FluentValidation.TestHelper;
using Pelican.Application.HubSpot.Commands.NewInstallation;
using Xunit;


namespace Pelican.Application.Test.HubSpot.Commands.NewInstallation;

public class NewInstallationCommandValidatorTests
{
	private readonly NewInstallationHubSpotCommandValidator _uut;

	public NewInstallationCommandValidatorTests()
	{
		_uut = new NewInstallationHubSpotCommandValidator();
	}

	[Fact]
	public void NewInstallationCommandValidator_EmptyCode_ReturnsError()
	{
		// Arrange
		NewInstallationHubSpotCommand command = new(String.Empty);

		// Act
		TestValidationResult<NewInstallationHubSpotCommand> result = _uut.TestValidate(command);

		// Assert
		result.ShouldHaveValidationErrorFor(command => command.Code);
	}

	[Fact]
	public void NewInstallationCommandValidator_NullCode_ReturnsError()
	{
		// Arrange
		NewInstallationHubSpotCommand command = new("code");

		// Act
		TestValidationResult<NewInstallationHubSpotCommand> result = _uut.TestValidate(command);

		// Assert
		result.ShouldNotHaveValidationErrorFor(command => command.Code);
	}
}
