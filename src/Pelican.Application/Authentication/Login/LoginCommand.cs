namespace Pelican.Application.Authentication.Login;
public sealed record LoginCommand(string Email, string Password) : ICommand;
