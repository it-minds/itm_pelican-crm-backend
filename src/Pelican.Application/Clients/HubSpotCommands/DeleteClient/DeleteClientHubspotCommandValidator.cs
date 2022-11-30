using FluentValidation;


namespace Pelican.Application.Clients.HubSpotCommands.DeleteClient;
internal sealed class DeleteClientHubspotCommandValidator : AbstractValidator<DeleteClientHubSpotCommand>
{
	public DeleteClientHubspotCommandValidator()
	{
		RuleFor(command => command.ObjectId).NotEmpty();
	}
}
