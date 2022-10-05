using System.Collections.Concurrent;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Pelican.Application.Abstractions.Messaging;
using Pelican.Application.Deals.Commands.DeleteDeal;
using Pelican.Application.Deals.Commands.UpdateDeal;
using Pelican.Application.HubSpot.Commands.NewInstallation;
using Pelican.Domain.Contracts;
using Pelican.Domain.Shared;
using Pelican.Presentation.Api.Abstractions;
using Pelican.Presentation.Api.Utilities;

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

		return result.IsSucces
			? Ok()
			: BadRequest(result.Error);
	}

	[HttpPost]
	[ServiceFilter(typeof(HubSpotValidationFilter))]
	public async Task<ActionResult> Hook(
		[FromBody] IEnumerable<WebHookRequest> requests,
		CancellationToken cancellationToken)
	{
		List<Result> results = new();
		IEnumerable<ICommand> commands = ConvertToCommands(requests);

		foreach (ICommand command in commands)
		{
			Result result = await Sender.Send(command, cancellationToken);
			results.Add(result);
		}

		return Result.FirstFailureOrSuccess(results.ToArray()).IsSucces
			? Ok()
			: BadRequest();
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
					//"contact.deletion" => new DeleteContactPropertyCommand(),
					"deal.deletion" => new DeleteDealCommand(request.ObjectId),
					//"contact.propertyChange" => new UpdateContactCommand(propertyChangeRequest.ObjectId, propertyChangeRequest.PropertyName, propertyChangeRequest.PropertyValue),
					"deal.propertyChange" => new UpdateDealCommand(Convert.ToInt64(request.SourceId.Split(":")[1]), request.ObjectId, request.PropertyName, request.PropertyValue),
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
