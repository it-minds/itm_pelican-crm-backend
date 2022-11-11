using MediatR;
using Microsoft.AspNetCore.Mvc;
using Pelican.Application.Abstractions.Messaging;
using Pelican.Application.HubSpot.Commands.NewInstallation;
using Pelican.Domain.Shared;
using Pelican.Presentation.Api.Abstractions;
using Pelican.Presentation.Api.Contracts;
using Pelican.Presentation.Api.Mapping;
using Pelican.Presentation.Api.Utilities.HubSpotHookValidation;

namespace Pelican.Presentation.Api.Controllers;

[Route("[controller]")]
//[EnableCors("HubSpot")]
internal sealed class HubSpotController : ApiController
{
	private readonly IRequestToCommandMapper _mapper;

	public HubSpotController(
		ISender sender,
		IRequestToCommandMapper requestToCommandMapper)
		: base(sender) => _mapper = requestToCommandMapper;

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
	[ServiceFilter(typeof(HubSpotValidationFilter))]
	public async Task<IActionResult> Hook(
		[FromBody] IReadOnlyCollection<WebHookRequest> requests,
		CancellationToken cancellationToken)
	{
		List<Result> results = new();
		IReadOnlyCollection<ICommand> commands = _mapper.ConvertToCommands(requests);

		if (commands.Count == 0)
		{
			return Ok();
		}

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
}

