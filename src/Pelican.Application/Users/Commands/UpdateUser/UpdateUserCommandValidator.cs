using FluentValidation;
using Pelican.Domain;

namespace Pelican.Application.Users.Commands.UpdateUser;

public sealed class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
	public UpdateUserCommandValidator()
	{
		RuleFor(u => u.User.Id)
			.NotEmpty();

		RuleFor(u => u.User.Email)
			.NotEmpty()
			.EmailAddress()
			.MaximumLength(StringLengths.Email);

		RuleFor(u => u.User.Name)
			.NotEmpty()
			.MaximumLength(StringLengths.Name);

		RuleFor(u => u.User.Role)
			.NotEmpty();
	}
}
