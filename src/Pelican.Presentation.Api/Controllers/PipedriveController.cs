using MediatR;
using Microsoft.AspNetCore.Mvc;
using Pelican.Application.Abstractions.Messaging;
using Pelican.Application.Deals.Commands.UpdateDeal;
using Pelican.Application.Pipedrive.Commands.NewInstallation;
using Pelican.Domain.Shared;
using Pelican.Presentation.Api.Abstractions;
using Pelican.Presentation.Api.Contracts.PipedriveWebHookRequests;

namespace Pelican.Presentation.Api.Controllers;

[Route("[controller]")]
public sealed class PipedriveController : ApiController
{
	public PipedriveController(ISender sender) : base(sender)
	{
	}

	[HttpGet("NewInstallation")]
	public async Task<IActionResult> NewInstallation(
		string code,
		CancellationToken cancellationToken)
	{
		NewInstallationPipedriveCommand newInstallation = new(
			code);

		Result result = await Sender.Send(newInstallation, cancellationToken);

		return result.IsSuccess
			? Redirect("https://it-minds.dk/")
			: BadRequest(result.Error);
	}

	[HttpPost("UpdateClient")]
	public async Task<IActionResult> UpdateDeal(
		[FromBody] UpdateDealResponse request)
	{
		List<Result> results = new();
		ICommand command = new UpdateDealPipedriveCommand(
			request.MetaProperties.SupplierPipedriveId,
			request.MetaProperties.SubscriptionAction,
			request.MetaProperties.SubscriptionObject,
			request.CurrentProperties.DealStatusId,
			request.CurrentProperties.DealDescription,
			request.CurrentProperties.DealName,
			request.CurrentProperties.LastContactDate,
			request.CurrentProperties.DealId,
			request.MetaProperties.UserId);

		results.Add(
			await Sender.Send(command, default));

		return results.First().IsSuccess
			? Ok()
			: BadRequest(results.First().Error);
	}
}
