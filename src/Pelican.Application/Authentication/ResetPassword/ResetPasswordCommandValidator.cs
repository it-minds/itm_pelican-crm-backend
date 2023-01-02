using FluentValidation;

namespace Pelican.Application.Authentication.ResetPassword;
public class ResetPasswordCommandValidator : AbstractValidator<ResetPasswordCommand>
{
	public ResetPasswordCommandValidator()
	{
		RuleFor(r => r.NewPassword)
		.NotEmpty().WithMessage("{PropertyName} cannot be empty")
		.MinimumLength(12).WithMessage("{PropertyName} length must be a minimum of {MinLength} characters")
		.Matches(@"[A-Z]+").WithMessage("{PropertyName} must contain at least one uppercase letter.")
		.Matches(@"[a-z]+").WithMessage("{PropertyName} must contain at least one lowercase letter.")
		.Matches(@"[0-9]+").WithMessage("{PropertyName} must contain at least one number.")
		.Matches(@"[][""!@$%^&*(){}:;<>,.?/+_=|'~\\-]").WithMessage("{PropertyName} must contain one or more special characters.");

		RuleFor(r => r.SSOToken)
			.NotEmpty();
	}
}
