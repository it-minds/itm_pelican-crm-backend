using FluentValidation;
using Pelican.Domain.Extensions;

namespace Pelican.Application.Users.Commands.UpdateUser;

public sealed class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
	public UpdateUserCommandValidator()
	{
		RuleFor(u => u.User.Id)
			.NotEmpty();

		RuleFor(u => u.User.Email)
			.AddUserEmailValidation();

		RuleFor(u => u.User.Name)
			.AddUserNameValidation();

		RuleFor(u => u.User.Role)
			.IsInEnum();
	}
}
