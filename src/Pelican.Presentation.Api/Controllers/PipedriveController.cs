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
	public async Task<IActionResult> DeleteDeal([FromBody] DeleteDealRequest request)
	{
		ICommand command = new DeleteDealPipedriveCommand(request.MetaProperties.ObjectId);

		Result result = await Sender.Send(command, default);

		return result.IsSuccess
			? Ok()
			: BadRequest(result.Error);
	}
}
