using Pelican.Application.Abstractions.Messaging;
using Pelican.Presentation.Api.Contracts;

namespace Pelican.Presentation.Api.Abstractions;

public interface IRequestToCommandMapper
{
	IReadOnlyCollection<ICommand> ConvertToCommands(IReadOnlyCollection<HubSpotWebHookRequest>? requests);
}
