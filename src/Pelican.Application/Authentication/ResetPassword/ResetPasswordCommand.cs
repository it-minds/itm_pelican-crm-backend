using Pelican.Application.Abstractions.Messaging;

namespace Pelican.Application.Authentication.ResetPassword;
public sealed record ResetPasswordCommand(string SSOToken, string NewPassword) : ICommand<UserTokenDto>;
