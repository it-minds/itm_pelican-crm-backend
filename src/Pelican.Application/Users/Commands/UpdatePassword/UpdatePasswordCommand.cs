
using Pelican.Application.Abstractions.Messaging;
using Pelican.Application.Authentication;

namespace Pelican.Application.Users.Commands.UpdatePassword;

public sealed record UpdatePasswordCommand(string Password) : ICommand<UserDto>;
