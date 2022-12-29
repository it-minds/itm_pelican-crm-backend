using Pelican.Application.Abstractions.Messaging;

namespace Pelican.Application.Authentication.CreateStandardUser;
public sealed record CreateStandardUserCommand(string Name, string Email, string Password) : ICommand;

