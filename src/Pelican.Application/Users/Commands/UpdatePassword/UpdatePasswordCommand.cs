
using Pelican.Application.Abstractions.Messaging;
using Pelican.Domain.Entities;

namespace Pelican.Application.Users.Commands.UpdatePassword;

public sealed record UpdatePasswordCommand(string Password) : ICommand<User>;
