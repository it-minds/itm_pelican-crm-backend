using FluentValidation;

namespace Pelican.Application.Users.Commands.UpdateUser;

public sealed class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
	public UpdateUserCommandValidator()
	{
		RuleFor(u => u.User.Id)
			.NotEmpty();
		RuleFor(u => u.User.Email)
			.NotEmpty()
			.EmailAddress();
		RuleFor(u => u.User.Name)
			.NotEmpty();
		RuleFor(u => u.User.Role)
			.NotEmpty();
	}
}
