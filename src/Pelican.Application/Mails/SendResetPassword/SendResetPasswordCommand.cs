using Pelican.Application.Abstractions.Messaging;

namespace Pelican.Application.Mails.SendResetPassword;
public sealed record SendResetPasswordCommand(
	string Email) : ICommand;
