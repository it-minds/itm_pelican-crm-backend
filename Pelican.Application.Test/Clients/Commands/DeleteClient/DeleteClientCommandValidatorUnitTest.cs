using FluentValidation.TestHelper;
using Pelican.Application.Clients.Commands.DeleteClient;
using Xunit;

namespace Pelican.Application.Test.Clients.Commands.DeleteClient;
public class DeleteClientCommandValidatorUnitTest
{
	private readonly DeleteClientCommandValidator _uut;
	public DeleteClientCommandValidatorUnitTest()
	{
		_uut = new();
	}

	[Fact]
	public void DeleteClientCommandValidator_EmptyId_ReturnsError()
	{
		// Arrange
		DeleteClientCommand command = new(0);

		// Act
		TestValidationResult<DeleteClientCommand> result = _uut.TestValidate(command);

		// Assert
		result.ShouldHaveValidationErrorFor(command => command.ObjectId);
	}

	[Theory]
	[InlineData(long.MaxValue)]
	[InlineData(long.MinValue)]
	public void DeleteClientCommandValidator_NoEmptyId_ReturnsNoError(long objecId)
	{
		// Arrange
		DeleteClientCommand command = new(objecId);

		// Act
		TestValidationResult<DeleteClientCommand> result = _uut.TestValidate(command);

		// Assert
		result.ShouldNotHaveValidationErrorFor(command => command.ObjectId);
	}
}
