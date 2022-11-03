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
	public void DeleteClientCommandCommandValidator_EmptyId_ReturnsError()
	{
		// Arrange
		DeleteClientCommand command = new(0);

		// Act
		TestValidationResult<DeleteClientCommand> result = _uut.TestValidate(command);

		// Assert
		result.ShouldHaveValidationErrorFor(command => command.ObjectId);
	}

	[Fact]
	public void DeleteClientCommandCommandValidator_NoEmptyId_ReturnsNoError()
	{
		// Arrange
		DeleteClientCommand command = new(long.MaxValue);

		// Act
		TestValidationResult<DeleteClientCommand> result = _uut.TestValidate(command);

		// Assert
		result.ShouldNotHaveValidationErrorFor(command => command.ObjectId);
	}

	[Fact]
	public void DeleteClientCommandCommandValidator_NegativeIdValue_ReturnsNoError()
	{
		// Arrange
		DeleteClientCommand command = new(long.MinValue);

		// Act
		TestValidationResult<DeleteClientCommand> result = _uut.TestValidate(command);

		// Assert
		result.ShouldNotHaveValidationErrorFor(command => command.ObjectId);
	}
}
