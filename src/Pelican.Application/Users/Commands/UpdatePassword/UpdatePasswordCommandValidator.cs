using FluentValidation;

namespace Pelican.Application.Users.Commands.UpdatePassword;

public sealed class UpdatePasswordCommandValidator : AbstractValidator<UpdatePasswordCommand>
{
	public UpdatePasswordCommandValidator()
	{
		RuleFor(r => r.Password)
			.NotEmpty().WithMessage("{PropertyName} cannot be empty")
			.MinimumLength(12).WithMessage("{PropertyName} length must be a minimum of {MinLength} characters")
			.Matches("[A-Z]+").WithMessage("{PropertyName} must contain at least one uppercase letter.")
			.Matches("[a-z]+").WithMessage("{PropertyName} must contain at least one lowercase letter.")
			.Matches("[0-9]+").WithMessage("{PropertyName} must contain at least one number.")
			.Matches(@"[][""!@$%^&*(){}:;<>,.?/+_=|'~\\-]").WithMessage("{PropertyName} must contain one or more special characters.");
	}
}
