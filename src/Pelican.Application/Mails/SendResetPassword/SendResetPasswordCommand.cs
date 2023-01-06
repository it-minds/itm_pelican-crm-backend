using Pelican.Application.Abstractions.Messaging;
using Pelican.Domain.Entities;

namespace Pelican.Application.Mails.SendResetPassword;
public sealed record SendResetPasswordCommand(
	string Email) : ICommand<User>;
