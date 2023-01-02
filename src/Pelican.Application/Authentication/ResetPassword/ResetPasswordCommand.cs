using Pelican.Application.Abstractions.Messaging;

namespace Pelican.Application.Authentication.ResetPassword;
public sealed record ResetPasswordCommand(string email) : ICommand<UserTokenDto>;
