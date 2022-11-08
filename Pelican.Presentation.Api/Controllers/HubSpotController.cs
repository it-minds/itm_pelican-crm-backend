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
		NewInstallationCommand newInstallation = new(code);

		Result result = await Sender.Send(
			newInstallation,
			cancellationToken);

		return result.IsSuccess
			? Redirect("https://it-minds.dk/")
			: BadRequest(result.Error);
	}

	[HttpPost]
	[ServiceFilter(typeof(HubSpotValidationFilter))]
	public async Task<IActionResult> Hook(
		[FromBody] IReadOnlyCollection<WebHookRequest> requests,
		CancellationToken cancellationToken)
	{
		List<Result> results = new();
		IReadOnlyCollection<ICommand> commands = requests.ConvertToCommands();

		foreach (ICommand command in commands)
		{
			results.Add(
				await Sender.Send(
					command,
					cancellationToken));
		}

		Result result = Result.FirstFailureOrSuccess(results.ToArray());

		return result.IsSuccess
			? Ok()
			: BadRequest(result.Error);
	}
}

