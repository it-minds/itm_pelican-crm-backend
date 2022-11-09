using FluentValidation.TestHelper;
using Pelican.Application.Deals.Commands.DeleteDeal;
using Xunit;


namespace Pelican.Application.Test.Deals.Commands.DeleteDeal;

public class DeleteDealCommandValidatorTests
{
	private readonly DeleteDealCommandValidator _uut;

	public DeleteDealCommandValidatorTests()
	{
		_uut = new DeleteDealCommandValidator();
	}

	[Fact]
	public void UpdateDealCommandValidator_EmptyId_ReturnsError()
	{
		// Arrange
		DeleteDealCommand command = new(0);

		// Act
		TestValidationResult<DeleteDealCommand> result = _uut.TestValidate(command);

		// Assert
		result.ShouldHaveValidationErrorFor(command => command.ObjectId);
	}

	[Fact]
	public void UpdateDealCommandValidator_NoEmptyId_ReturnsNoError()
	{
		// Arrange
		DeleteDealCommand command = new(1);

		// Act
		TestValidationResult<DeleteDealCommand> result = _uut.TestValidate(command);

		// Assert
		result.ShouldNotHaveValidationErrorFor(command => command.ObjectId);
	}
}
