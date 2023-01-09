using Pelican.Application.Abstractions.Messaging;
using Pelican.Application.Authentication;

namespace Pelican.Application.Users.Commands.CreateAdmin;
public sealed record CreateAdminCommand(string Name, string Email, string Password) : ICommand<UserDto>;
