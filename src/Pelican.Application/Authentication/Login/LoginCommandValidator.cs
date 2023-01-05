using FluentValidation;
using Pelican.Domain.Extensions;

namespace Pelican.Application.Authentication.Login;
internal sealed class LoginCommandValidator : AbstractValidator<LoginCommand>
{
	public LoginCommandValidator()
	{
		RuleFor(command => command.Email)
			.AddUserEmailValidation();

		RuleFor(command => command.Password)
			.AddUserPasswordValidation();
	}
}
