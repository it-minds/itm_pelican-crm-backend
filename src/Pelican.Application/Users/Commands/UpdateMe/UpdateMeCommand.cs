using Pelican.Application.Abstractions.Messaging;
using Pelican.Application.Authentication;

namespace Pelican.Application.Users.Commands.UpdateMe;

public record UpdateMeCommand(UserDto User) : ICommand<UserDto>;
