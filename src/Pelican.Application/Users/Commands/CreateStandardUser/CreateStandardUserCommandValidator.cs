using FluentValidation;
using Pelican.Domain.Extensions;

namespace Pelican.Application.Users.Commands.CreateStandardUser;
public class CreateStandardUserCommandValidator : AbstractValidator<CreateStandardUserCommand>
{
	public CreateStandardUserCommandValidator()
	{
		RuleFor(c => c.Email)
			.AddUserEmailValidation();
		RuleFor(c => c.Password)
			.AddUserPasswordValidation();
		RuleFor(c => c.Name)
			.AddUserNameValidation();
	}
}
