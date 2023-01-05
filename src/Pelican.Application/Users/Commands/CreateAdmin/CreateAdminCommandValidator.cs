using FluentValidation;
using Pelican.Domain.Extensions;

namespace Pelican.Application.Users.Commands.CreateAdmin;
public class CreateAdminCommandValidator : AbstractValidator<CreateAdminCommand>
{
	public CreateAdminCommandValidator()
	{
		RuleFor(c => c.Email)
			.AddUserEmailValidation();
		RuleFor(c => c.Password)
			.AddUserPasswordValidation();
		RuleFor(c => c.Name)
			.AddUserNameValidation();
	}
}
