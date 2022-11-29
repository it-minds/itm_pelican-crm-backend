using MediatR;
using Microsoft.AspNetCore.Mvc;
using Pelican.Application.Abstractions.Messaging;
using Pelican.Application.Deals.Commands.UpdateDeal;
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

	[HttpPost("UpdateClient")]
	public async Task<IActionResult> UpdateDeal(
		[FromBody] UpdateDealResponse request,
		CancellationToken cancellationToken)
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
			await Sender.Send(command, cancellationToken));

		Result result = Result.FirstFailureOrSuccess(results.ToArray());

		return result.IsSuccess
			? Ok()
			: BadRequest(result.Error);
	}
}
