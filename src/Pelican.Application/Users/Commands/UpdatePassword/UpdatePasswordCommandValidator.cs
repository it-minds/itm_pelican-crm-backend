using FluentValidation;

namespace Pelican.Application.Users.Commands.UpdatePassword;

public sealed class UpdatePasswordCommandValidator : AbstractValidator<UpdatePasswordCommand>
{
	public UpdatePasswordCommandValidator()
	{
		RuleFor(c => c.Password)
			.NotEmpty().WithMessage("Your password cannot be empty")
			.MinimumLength(8).WithMessage("Your password length must be a minimum of 8 characters")
			.Matches("[A-Z]+").WithMessage("Your password must contain at least one uppercase letter.")
			.Matches("[a-z]+").WithMessage("Your password must contain at least one lowercase letter.")
			.Matches("[0-9]+").WithMessage("Your password must contain at least one number.");
	}
}
