using MediatR;
using Microsoft.AspNetCore.Mvc;
using Pelican.Application.Abstractions.Messaging;
using Pelican.Application.Deals.PipedriveCommands.DeleteDeal;
using Pelican.Application.Deals.PipedriveCommands.UpdateDeal;
using Pelican.Domain.Shared;
using Pelican.Presentation.Api.Abstractions;
using Pelican.Presentation.Api.Contracts.PipedriveWebHookRequests.DeleteDealRequest;
using Pelican.Presentation.Api.Contracts.PipedriveWebHookRequests.UpdateDeal;

namespace Pelican.Presentation.Api.Controllers;

[Route("[controller]")]
public sealed class PipedriveController : ApiController
{
	public PipedriveController(ISender sender) : base(sender)
	{
	}

	[HttpPost("UpdateClient")]
	public async Task<IActionResult> UpdateDeal(
		[FromBody] UpdateDealRequest request)
	{
		ICommand command = new UpdateDealPipedriveCommand(
			request.MetaProperties.SupplierPipedriveId,
			request.MetaProperties.ObjectId,
			request.MetaProperties.UserId,
			request.CurrentProperties.DealStatusId,
			request.CurrentProperties.DealDescription,
			request.CurrentProperties.DealName,
			request.CurrentProperties.LastContactDate,
			null,
			null);

		results.Add(
			await Sender.Send(command, default));

		return results.First().IsSuccess
			? Ok()
			: BadRequest(results.First().Error);
	}
	[HttpPost("DeleteClient")]
	public async Task<IActionResult> DeleteDeal([FromBody] DeleteDealRequest request)
	{
		List<Result> results = new();
		ICommand command = new DeleteDealPipedriveCommand(
			request.MetaProperties.SupplierPipedriveId,
			request.MetaProperties.ObjectId,
			request.MetaProperties.UserId);

		results.Add(
			await Sender.Send(command, default));

		return results.First().IsSuccess
			? Ok()
			: BadRequest(results.First().Error);
	}
}
