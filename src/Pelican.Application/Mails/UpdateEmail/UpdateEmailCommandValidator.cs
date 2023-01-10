using FluentValidation;
using Pelican.Domain;

namespace Pelican.Application.Mails.UpdateEmail;

public class UpdateEmailCommandValidator : AbstractValidator<UpdateEmailCommand>
{
	public UpdateEmailCommandValidator()
	{
		RuleFor(e => e.Email.Id)
			.NotEmpty();

		RuleFor(e => e.Email.Name)
			.NotEmpty()
			.MaximumLength(StringLengths.Name);

		RuleFor(e => e.Email.Subject)
			.NotEmpty()
			.MaximumLength(StringLengths.SubjectLine);

		RuleFor(e => e.Email.Heading1)
			.NotEmpty()
			.MaximumLength(StringLengths.Heading);

		RuleFor(e => e.Email.Heading2)
			.NotEmpty()
			.MaximumLength(StringLengths.Heading);

		RuleFor(e => e.Email.Heading3)
			.NotEmpty()
			.MaximumLength(StringLengths.Heading);

		RuleFor(e => e.Email.Paragraph1)
			.NotEmpty()
			.MaximumLength(StringLengths.Paragraph);

		RuleFor(e => e.Email.Paragraph2)
			.NotEmpty()
			.MaximumLength(StringLengths.Paragraph);

		RuleFor(e => e.Email.Paragraph3)
			.NotEmpty()
			.MaximumLength(StringLengths.Paragraph);

		RuleFor(e => e.Email.CtaButtonText)
			.NotEmpty()
			.MaximumLength(StringLengths.CtaButtonText);
	}
}
