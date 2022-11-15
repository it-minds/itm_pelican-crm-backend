using FluentValidation.TestHelper;
using Pelican.Application.Contacts.Commands.DeleteContact;
using Xunit;


namespace Pelican.Application.Test.Contacts.Commands.DeleteContact;

public class DeleteContactCommandValidatorTests
{
	private readonly DeleteContactCommandValidator _uut;

	public DeleteContactCommandValidatorTests()
	{
		_uut = new DeleteContactCommandValidator();
	}

	[Fact]
	public void UpdateContactCommandValidator_EmptyId_ReturnsError()
	{
		// Arrange
		DeleteContactCommand command = new(0);

		// Act
		TestValidationResult<DeleteContactCommand> result = _uut.TestValidate(command);

		// Assert
		result.ShouldHaveValidationErrorFor(command => command.ObjectId);
	}

	[Fact]
	public void UpdateContactCommandValidator_NoEmptyId_ReturnsNoError()
	{
		// Arrange
		DeleteContactCommand command = new(1);

		// Act
		TestValidationResult<DeleteContactCommand> result = _uut.TestValidate(command);

		// Assert
		result.ShouldNotHaveValidationErrorFor(command => command.ObjectId);
	}
}
