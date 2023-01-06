using Pelican.Application.Abstractions.Messaging;
using Pelican.Application.Authentication;

namespace Pelican.Application.Users.Commands.CreateStandardUser;
public sealed record CreateStandardUserCommand(string Name, string Email, string Password) : ICommand<UserDto>;

