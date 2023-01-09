using Pelican.Application.Abstractions.Messaging;
using Pelican.Application.Authentication;

namespace Pelican.Application.Users.Commands.UpdateUser;

public sealed record UpdateUserCommand(UserDto User) : ICommand<UserDto>;
