using FluentValidation.TestHelper;
using Pelican.Application.Deals.HubSpotCommands.DeleteDeal;
using Xunit;


namespace Pelican.Application.Test.Deals.Commands.DeleteDeal;

public class DeleteDealCommandValidatorTests
{
	private readonly DeleteDealHubSpotCommandValidator _uut;

	public DeleteDealCommandValidatorTests()
	{
		_uut = new DeleteDealHubSpotCommandValidator();
	}

	[Fact]
	public void UpdateDealCommandValidator_EmptyId_ReturnsError()
	{
		// Arrange
		DeleteDealHubSpotCommand command = new(0);

		// Act
		TestValidationResult<DeleteDealHubSpotCommand> result = _uut.TestValidate(command);

		// Assert
		result.ShouldHaveValidationErrorFor(command => command.ObjectId);
	}

	[Fact]
	public void UpdateDealCommandValidator_NoEmptyId_ReturnsNoError()
	{
		// Arrange
		DeleteDealHubSpotCommand command = new(1);

		// Act
		TestValidationResult<DeleteDealHubSpotCommand> result = _uut.TestValidate(command);

		// Assert
		result.ShouldNotHaveValidationErrorFor(command => command.ObjectId);
	}
}
