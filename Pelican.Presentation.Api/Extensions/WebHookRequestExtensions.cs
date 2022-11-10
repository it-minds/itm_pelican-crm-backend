using System.Collections.Concurrent;
using Pelican.Application.Abstractions.Messaging;
using Pelican.Application.Contacts.Commands.DeleteContact;
using Pelican.Application.Contacts.Commands.UpdateContact;
using Pelican.Application.Deals.Commands.DeleteDeal;
using Pelican.Application.Deals.Commands.UpdateDeal;
using Pelican.Presentation.Api.Contracts;

namespace Pelican.Presentation.Api.Extensions;

internal static class WebHookRequestExtensions
{
	internal static IReadOnlyCollection<ICommand> ConvertToCommands(this IReadOnlyCollection<WebHookRequest>? requests)
	{
		if (requests is null)
		{
			return new List<ICommand>();
		}

		BlockingCollection<ICommand> commands = new();

		requests
			.AsParallel()
			.ForAll(request =>
			{
				ICommand? command = request.SubscriptionType switch
				{
					"contact.deletion" => new DeleteContactCommand(
						request.ObjectId),
					"deal.deletion" => new DeleteDealCommand(
						request.ObjectId),
					"contact.propertyChange" => new UpdateContactCommand(
						request.ObjectId,
						request.PortalId,
						request.PropertyName,
						request.PropertyValue),
					"deal.propertyChange" => new UpdateDealCommand(
						request.ObjectId,
						request.PortalId,
						request.PropertyName,
						request.PropertyValue),
					_ => null
				};

				if (command is not null)
				{
					commands.Add(command);
				}
			});

		return commands;
	}
}
