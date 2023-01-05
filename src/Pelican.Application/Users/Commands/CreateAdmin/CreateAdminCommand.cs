using Pelican.Application.Abstractions.Messaging;
using Pelican.Domain.Entities;

namespace Pelican.Application.Users.Commands.CreateAdmin;
public sealed record CreateAdminCommand(string Name, string Email, string Password) : ICommand<User>;
