using FluentValidation;

namespace Pelican.Application.Authentication.Login;
internal sealed class LoginCommandValidator : AbstractValidator<LoginCommand>
{
	public LoginCommandValidator()
	{
		RuleFor(command => command.Email).NotEmpty();
		RuleFor(command => command.Password).NotEmpty();
	}
}
