using Pelican.Application.Abstractions.Messaging;
using Pelican.Application.Authentication;
using Pelican.Domain.Entities;

namespace Pelican.Application.Users.Commands.UpdateUser;

public sealed record UpdateUserCommand(UserDto User) : ICommand<User>;
