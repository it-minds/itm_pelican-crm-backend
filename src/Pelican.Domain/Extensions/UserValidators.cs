using FluentValidation;

namespace Pelican.Domain.Extensions;
public static class UserValidators
{
	public static IRuleBuilder<T, string> AddUserNameValidation<T>(this IRuleBuilder<T, string> ruleBuilder)
	{
		var options = ruleBuilder
			.NotEmpty().WithMessage("{PropertyName} cannot be empty.")
			.MaximumLength(StringLengths.Name).WithMessage("{PropertyName} cannot be longer than " + $"{StringLengths.Name}.");
		return options;
	}

	public static IRuleBuilder<T, string> AddUserPasswordValidation<T>(this IRuleBuilder<T, string> ruleBuilder)
	{
		var options = ruleBuilder
			.NotEmpty().WithMessage("{PropertyName} cannot be empty.")
			.MinimumLength(12).WithMessage("{PropertyName} length must be a minimum of 12 characters.")
			.MaximumLength(StringLengths.Password).WithMessage("{PropertyName} cannot be longer than " + $"{StringLengths.Password}.")
			.Matches("[A-Z]+").WithMessage("{PropertyName} must contain at least one uppercase letter.")
			.Matches("[a-z]+").WithMessage("{PropertyName} must contain at least one lowercase letter.")
			.Matches("[0-9]+").WithMessage("{PropertyName} must contain at least one number.")
			.Matches(@"[][""!@$%^&*(){}:;<>,.?/+_=|'~\\-]").WithMessage("{PropertyName} must contain one or more special characters.");
		return options;
	}

	public static IRuleBuilder<T, string> AddUserEmailValidation<T>(this IRuleBuilder<T, string> ruleBuilder)
	{
		var options = ruleBuilder
			.NotEmpty().WithMessage("{PropertyName} cannot be empty.")
			.EmailAddress().WithMessage("{PropertyName} is not a valid email.")
			.MaximumLength(StringLengths.Email).WithMessage("{PropertyName} cannot be longer than " + $"{StringLengths.Email}.");
		return options;
	}
}
