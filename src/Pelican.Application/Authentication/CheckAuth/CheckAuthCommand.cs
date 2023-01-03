using Pelican.Application.Abstractions.Messaging;

namespace Pelican.Application.Authentication.CheckAuth;
//TODO Add authenticated attribute when this is added.
//[Authenticated]
public sealed record CheckAuthCommand() : ICommand<UserDto>;
