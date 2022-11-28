﻿using HotChocolate.Language.Visitors;
using Pelican.Application.Abstractions.Messaging;
using Pelican.Application.AccountManagers.Commands.ValidateWebhookUserId;
using Pelican.Application.Clients.Commands.DeleteClient;
using Pelican.Application.Clients.Commands.UpdateClient;
using Pelican.Application.Contacts.Commands.UpdateContact;
using Pelican.Application.Deals.Commands.DeleteDeal;
using Pelican.Application.Deals.Commands.UpdateDeal;
using Pelican.Presentation.Api.Abstractions;
using Pelican.Presentation.Api.Contracts;

namespace Pelican.Presentation.Api.Mapping;

internal sealed class WebHookRequestsToCommands : IRequestToCommandMapper
{
	public IReadOnlyCollection<ICommand> ConvertToCommands(IReadOnlyCollection<WebHookRequest>? requests)
	{
		if (requests is null)
		{
			return new List<ICommand>();
		}

		List<ICommand> commands = new();

		foreach (var request in requests
			.GroupBy(request => request.SourceId)
			.Select(group => group.First())
			.Where(group => group.SourceId.StartsWith("userId")))
		{
			if (!long.TryParse(request.SourceId[7..], out long accountManagerHubspotId))
			{
				continue;
			}

			commands.Add(new ValidateWebhookUserIdCommand(
				accountManagerHubspotId,
				request.SupplierHubSpotId));
		}

		foreach (WebHookRequest request in requests)
		{
			commands.Add(request.SubscriptionType switch
			{
				"deal.deletion" => new DeleteDealCommand(
					request.ObjectId),
				"company.deletion" => new DeleteClientCommand(
					request.ObjectId),
				"contact.propertyChange" => new UpdateContactCommand(
					request.ObjectId,
					request.SupplierHubSpotId,
					request.PropertyName,
					request.PropertyValue),
				"deal.propertyChange" => new UpdateDealCommand(
					request.ObjectId,
					request.SupplierHubSpotId,
					request.PropertyName,
					request.PropertyValue),
				"company.propertyChange" => new UpdateClientCommand(
					request.ObjectId,
					request.SupplierHubSpotId,
					request.PropertyName,
					request.PropertyValue),
				_ => throw new InvalidDataException("Receiving unhandled event"),
			});
		}

		return commands;
	}
}