using FluentValidation;
using Pelican.Domain;
using Pelican.Domain.Extensions;

namespace Pelican.Application.Authentication.Login;
internal sealed class LoginCommandValidator : AbstractValidator<LoginCommand>
{
	public LoginCommandValidator()
	{
		RuleFor(command => command.Email)
			.AddUserEmailValidation();

		RuleFor(command => command.Password)
			.NotEmpty()
			.MaximumLength(StringLengths.Password).WithMessage("{PropertyName} cannot be longer than " + $"{StringLengths.Password}.");
	}
}
