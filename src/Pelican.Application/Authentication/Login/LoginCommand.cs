using Pelican.Application.Abstractions.Messaging;

namespace Pelican.Application.Authentication.Login;
public sealed record LoginCommand(string email, string password) : ICommand;
