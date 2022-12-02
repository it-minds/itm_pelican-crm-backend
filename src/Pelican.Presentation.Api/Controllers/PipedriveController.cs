using MediatR;
using Microsoft.AspNetCore.Mvc;
using Pelican.Application.Abstractions.Messaging;
using Pelican.Application.AccountManagers.PipedriveCommands.UpdateAccountManager;
using Pelican.Application.Clients.PipedriveClientCommands;
using Pelican.Application.Deals.PipedriveCommands.DeleteDeal;
using Pelican.Application.Deals.PipedriveCommands.UpdateDeal;
using Pelican.Application.Pipedrive.Commands.NewInstallation;
using Pelican.Domain.Shared;
using Pelican.Presentation.Api.Abstractions;
using Pelican.Presentation.Api.Contracts.PipedriveWebHookRequests.AccountManager.UpdateAccountManager;
using Pelican.Presentation.Api.Contracts.PipedriveWebHookRequests.Deal.DeleteDeal;
using Pelican.Presentation.Api.Contracts.PipedriveWebHookRequests.Deal.UpdateDeal;
using Pelican.Presentation.Api.Contracts.PipedriveWebHookRequests.UpdateClient;

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

	[HttpPost("UpdateDeal")]
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

		Result result = await Sender.Send(command, default);

		return result.IsSuccess
			? Ok()
			: BadRequest(result.Error);
	}

	[HttpPost("DeleteDeal")]
	public async Task<IActionResult> DeleteDeal([FromBody] DeleteDealRequest request)
	{
		ICommand command = new DeleteDealPipedriveCommand(request.MetaProperties.ObjectId);

		Result result = await Sender.Send(command, default);

		return result.IsSuccess
			? Ok()
			: BadRequest(result.Error);
	}

	[HttpPost("UpdateClient")]
	public async Task<IActionResult> UpdateClient(
		[FromBody] UpdateClientRequest request)
	{
		ICommand command = new UpdateClientPipedriveCommand(
			request.MetaProperties.SupplierPipedriveId,
			request.MetaProperties.ObjectId,
			request.MetaProperties.UserId,
			request.CurrentProperties.ClientName,
			null,
			request.CurrentProperties.OfficeLocation,
			null
			);

		Result result = await Sender.Send(command, default);

		return result.IsSuccess
			? Ok()
			: BadRequest(result.Error);
	}

	[HttpPost("DeleteClient")]
	public async Task<IActionResult> DeleteClient([FromBody] DeleteClientRequest request)
	{
		ICommand command = new DeleteClientPipedriveCommand(request.MetaProperties.ObjectId);

		Result result = await Sender.Send(command, default);

		return result.IsSuccess
			? Ok()
			: BadRequest(result.Error);
	}


	[HttpPost("UpdateAccountManager")]
	public async Task<IActionResult> UpdateAccountManager(
		[FromBody] UpdateAccountManagerRequest request)
	{
		var fullNameSplit = request.CurrentProperties.AccountManagerFullName.Split(' ');
		string firstName = string.Join(" ", fullNameSplit.Take(fullNameSplit.Length - 1));
		string? lastName = fullNameSplit.LastOrDefault();
		ICommand command = new UpdateAccountManagerPipedriveCommand(
			request.MetaProperties.SupplierPipedriveId,
			request.MetaProperties.ObjectId,
			request.MetaProperties.UserId,
			firstName,
			lastName,
			request.CurrentProperties.PictureUrl,
			request.CurrentProperties.PhoneNumber,
			request.CurrentProperties.Email,
			null);

		Result result = await Sender.Send(command, default);

		return result.IsSuccess
			? Ok()
			: BadRequest(result.Error);
	}

	[HttpPost("DeleteAccountManager")]
	public async Task<IActionResult> DeleteAccountManager([FromBody] DeleteAccountManagerRequest request)
	{
		ICommand command = new DeleteAccountManagerPipedriveCommand(request.MetaProperties.ObjectId);

		Result result = await Sender.Send(command, default);

		return result.IsSuccess
			? Ok()
			: BadRequest(result.Error);
	}
	[HttpPost("UpdateContact")]
	public async Task<IActionResult> UpdateContact(
		[FromBody] UpdateContactRequest request)
	{
		ICommand command = new UpdateContactPipedriveCommand(
			request.MetaProperties.SupplierPipedriveId,
			request.MetaProperties.ObjectId,
			request.MetaProperties.UserId,
			request.CurrentProperties.FirstName,
			request.CurrentProperties.LastName,
			request.CurrentProperties.PictureUrl,
			request.CurrentProperties.PhoneNumber?.Where(x => x.Primary == true).FirstOrDefault()?.Value,
			request.CurrentProperties.Email?.Where(x => x.Primary == true).FirstOrDefault()?.Value,
			null);

		Result result = await Sender.Send(command, default);

		return result.IsSuccess
			? Ok()
			: BadRequest(result.Error);
	}


	[HttpPost("UpdateAccountManager")]
	public async Task<IActionResult> UpdateAccountManager(
		[FromBody] UpdateAccountManagerRequest request)
	{
		var fullNameSplit = request.CurrentProperties.AccountManagerFullName.Split(' ');
		string firstName = string.Join(" ", fullNameSplit.Take(fullNameSplit.Length - 1));
		string? lastName = fullNameSplit.LastOrDefault();
		ICommand command = new UpdateAccountManagerPipedriveCommand(
			request.MetaProperties.SupplierPipedriveId,
			request.MetaProperties.ObjectId,
			request.MetaProperties.UserId,
			firstName,
			lastName,
			request.CurrentProperties.PictureUrl,
			request.CurrentProperties.PhoneNumber,
			request.CurrentProperties.Email,
			null);

		Result result = await Sender.Send(command, default);

		return result.IsSuccess
			? Ok()
			: BadRequest(result.Error);
	}
}
