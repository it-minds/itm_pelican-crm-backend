using FluentValidation;
using Pelican.Domain.Extensions;

namespace Pelican.Application.Mails.SendResetPassword;
public class SendResetPasswordCommandValidator : AbstractValidator<SendResetPasswordCommand>
{
	public SendResetPasswordCommandValidator()
	{
		RuleFor(x => x.Email).AddUserEmailValidation();
	}
}
