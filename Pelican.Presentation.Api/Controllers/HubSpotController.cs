using System.Collections.Concurrent;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Pelican.Application.Abstractions.Messaging;
using Pelican.Application.Clients.Commands.DeleteClient;
using Pelican.Application.Clients.Commands.UpdateClient;
using Pelican.Application.Deals.Commands.DeleteDeal;
using Pelican.Application.Deals.Commands.UpdateDeal;
using Pelican.Application.HubSpot.Commands.NewInstallation;
using Pelican.Domain.Shared;
using Pelican.Presentation.Api.Abstractions;
using Pelican.Presentation.Api.Contracts;

namespace Pelican.Presentation.Api.Controllers;

[Route("[controller]")]
//[EnableCors("HubSpot")]
public sealed class HubSpotController : ApiController
{
	public HubSpotController(ISender sender)
		: base(sender)
	{ }

	[HttpGet]
	public async Task<IActionResult> NewInstallation(
		string code,
		CancellationToken cancellationToken)
	{
		NewInstallationCommand newInstallation = new(
			code);

		Result result = await Sender.Send(newInstallation, cancellationToken);

		return result.IsSuccess
			? Redirect("https://it-minds.dk/")
			: BadRequest(result.Error);
	}

	[HttpPost]
	//[ServiceFilter(typeof(HubSpotValidationFilter))]
	public async Task<IActionResult> Hook(
		[FromBody] IEnumerable<WebHookRequest> requests,
		CancellationToken cancellationToken)
	{
		List<Result> results = new();
		IEnumerable<ICommand> commands = ConvertToCommands(requests);

		foreach (ICommand command in commands)
		{
			results.Add(
				await Sender.Send(command, cancellationToken));
		}

		Result result = Result.FirstFailureOrSuccess(results.ToArray());

		return result.IsSuccess
			? Ok()
			: BadRequest(result.Error);
	}

	private static IEnumerable<ICommand> ConvertToCommands(IEnumerable<WebHookRequest> requests)
	{
		BlockingCollection<ICommand> commands = new();

		requests
			.AsParallel()
			.ForAll(request =>
			{
				ICommand? command = request.SubscriptionType switch
				{
					//"contact.deletion" => new DeleteContactPropertyCommand(request.ObjectId),
					"deal.deletion" => new DeleteDealCommand(request.ObjectId),
					"company.propertyChange" => new UpdateClientCommand(request.ObjectId, request.PortalId, request.PropertyName, request.PropertyValue),
					"company.deletion" => new DeleteClientCommand(request.ObjectId),
					//"contact.propertyChange" => new UpdateContactCommand(propertyChangeRequest.ObjectId, propertyChangeRequest.PropertyName, propertyChangeRequest.PropertyValue),
					"deal.propertyChange" => new UpdateDealCommand(request.ObjectId, request.PortalId, request.PropertyName, request.PropertyValue),
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
