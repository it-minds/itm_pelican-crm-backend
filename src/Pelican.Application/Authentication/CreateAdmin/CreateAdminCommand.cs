using Pelican.Application.Abstractions.Messaging;

namespace Pelican.Application.Authentication.CreateAdmin;
public sealed record CreateAdminCommand(string Name, string Email, string Password) : ICommand;
