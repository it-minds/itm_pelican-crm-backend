namespace Pelican.Application.Authentication.ResetPassword;
public class ResetPasswordCommandValidator : AbstractValidator<ResetPasswordCommand>
{
	public ResetPasswordCommandValidator()
	{
		RuleFor(r => r.NewPassword)
			.AddUserPasswordValidation();

		RuleFor(r => r.SSOToken)
			.NotEmpty();
	}
}
