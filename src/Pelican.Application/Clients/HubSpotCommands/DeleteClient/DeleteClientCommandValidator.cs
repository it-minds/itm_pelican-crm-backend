using FluentValidation;


namespace Pelican.Application.Clients.HubSpotCommands.DeleteClient;
internal sealed class DeleteClientCommandValidator : AbstractValidator<DeleteClientCommand>
{
	public DeleteClientCommandValidator()
	{
		RuleFor(command => command.ObjectId).NotEmpty();
	}
}
