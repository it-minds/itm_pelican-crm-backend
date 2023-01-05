using FluentValidation;
using Pelican.Domain.Extensions;

namespace Pelican.Application.Users.Commands.UpdatePassword;

public sealed class UpdatePasswordCommandValidator : AbstractValidator<UpdatePasswordCommand>
{
	public UpdatePasswordCommandValidator()
	{
		RuleFor(r => r.Password)
			.AddUserPasswordValidation();
	}
}
