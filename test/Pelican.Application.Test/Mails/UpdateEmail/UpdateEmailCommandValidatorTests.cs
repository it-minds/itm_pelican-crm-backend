using FluentValidation.TestHelper;
using Pelican.Application.Mails.UpdateEmail;
using Pelican.Domain;
using Xunit;

namespace Pelican.Application.Test.Mails.UpdateEmail;

public class UpdateEmailCommandValidatorTests
{
	private readonly UpdateEmailCommandValidator _uut = new();

	[Fact]
	public void UpdateEmailCommandValidator_IdAndStringsEmpty_ReturnsError()
	{
		// Arrange
		UpdateEmailCommand command = new(
			new()
			{
				Id = Guid.Empty,
				Name = string.Empty,
				Subject = string.Empty,
				Heading1 = string.Empty,
				Paragraph1 = string.Empty,
				Heading2 = string.Empty,
				Paragraph2 = string.Empty,
				Heading3 = string.Empty,
				Paragraph3 = string.Empty,
				CtaButtonText = string.Empty,
			}
		);

		// Act
		var result = _uut.TestValidate(command);

		// Assert
		result.ShouldHaveValidationErrorFor(command => command.Email.Id);
		result.ShouldHaveValidationErrorFor(command => command.Email.Name);
		result.ShouldHaveValidationErrorFor(command => command.Email.Subject);
		result.ShouldHaveValidationErrorFor(command => command.Email.Heading1);
		result.ShouldHaveValidationErrorFor(command => command.Email.Paragraph1);
		result.ShouldHaveValidationErrorFor(command => command.Email.Heading2);
		result.ShouldHaveValidationErrorFor(command => command.Email.Paragraph2);
		result.ShouldHaveValidationErrorFor(command => command.Email.Heading3);
		result.ShouldHaveValidationErrorFor(command => command.Email.Paragraph3);
		result.ShouldHaveValidationErrorFor(command => command.Email.Paragraph1);
	}

	[Fact]
	public void UpdateEmailCommandValidator_StringsToLong_ReturnsError()
	{
		// Arrange
		UpdateEmailCommand command = new(
			new()
			{
				Id = Guid.NewGuid(),
				Name = new string('x', StringLengths.Name + 1),
				Subject = new string('x', StringLengths.SubjectLine + 1),
				Heading1 = new string('x', StringLengths.Heading + 1),
				Paragraph1 = new string('x', StringLengths.Paragraph + 1),
				Heading2 = new string('x', StringLengths.Heading + 1),
				Paragraph2 = new string('x', StringLengths.Paragraph + 1),
				Heading3 = new string('x', StringLengths.Heading + 1),
				Paragraph3 = new string('x', StringLengths.Paragraph + 1),
				CtaButtonText = new string('x', StringLengths.CtaButtonText + 1),
			}
		);

		// Act
		var result = _uut.TestValidate(command);

		// Assert
		result.ShouldHaveValidationErrorFor(command => command.Email.Name);
		result.ShouldHaveValidationErrorFor(command => command.Email.Subject);
		result.ShouldHaveValidationErrorFor(command => command.Email.Heading1);
		result.ShouldHaveValidationErrorFor(command => command.Email.Paragraph1);
		result.ShouldHaveValidationErrorFor(command => command.Email.Heading2);
		result.ShouldHaveValidationErrorFor(command => command.Email.Paragraph2);
		result.ShouldHaveValidationErrorFor(command => command.Email.Heading3);
		result.ShouldHaveValidationErrorFor(command => command.Email.Paragraph3);
		result.ShouldHaveValidationErrorFor(command => command.Email.Paragraph1);
	}

	[Fact]
	public void UpdateEmailCommandValidator_NoErrors_ReturnsNoError()
	{
		// Arrange
		UpdateEmailCommand command = new(
			new()
			{
				Id = Guid.NewGuid(),
				Name = new string('x', StringLengths.Name),
				Subject = new string('x', StringLengths.SubjectLine),
				Heading1 = new string('x', StringLengths.Heading),
				Paragraph1 = new string('x', StringLengths.Paragraph),
				Heading2 = new string('x', StringLengths.Heading),
				Paragraph2 = new string('x', StringLengths.Paragraph),
				Heading3 = new string('x', StringLengths.Heading),
				Paragraph3 = new string('x', StringLengths.Paragraph),
				CtaButtonText = new string('x', StringLengths.CtaButtonText),
			}
		);

		// Act
		var result = _uut.TestValidate(command);

		// Assert
		result.ShouldNotHaveAnyValidationErrors();
	}
}
