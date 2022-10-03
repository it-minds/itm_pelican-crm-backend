using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Pelican.Application.HubSpot.Command.NewInstallation;
using Pelican.Domain.Shared;
using Pelican.Infrastructure.HubSpot;
using Pelican.Presentation.Api.Abstractions;
using Pelican.Presentation.Api.Contracts;
using Pelican.Presentation.Api.Utilities;

namespace Pelican.Presentation.Api.Controllers;

[ApiController]
[Route("[controller]")]
//[EnableCors("HubSpot")]
public sealed class HubSpotController : ApiController
{
	private readonly IOptions<HubSpotSettings> _hubSpotSettings;

	public HubSpotController(ISender sender, IOptions<HubSpotSettings> hubSpotSettings) : base(sender)
	{
		_hubSpotSettings = hubSpotSettings;
	}

	[HttpGet]
	public async Task<IActionResult> NewInstallation(
		string code,
		CancellationToken cancellationToken)
	{
		if (_hubSpotSettings is null) throw new NullReferenceException();

		NewInstallationCommand newInstallation = new(
			code,
			_hubSpotSettings.Value.BaseUrl ?? throw new NullReferenceException(),
			_hubSpotSettings.Value.App?.ClientId ?? throw new NullReferenceException(),
			"https://eomyft7gbubnxim.m.pipedream.net",
			_hubSpotSettings.Value.App.ClientSecret ?? throw new NullReferenceException());

		Result result = await Sender.Send(newInstallation, cancellationToken);

		return result.IsSucces
			? Ok()
			: BadRequest();
	}

	[HttpPost]
	[ServiceFilter(typeof(HubSpotValidationFilter))]
	public async Task<ActionResult> Hook(
		[FromBody] IEnumerable<WebHookRequest> payloads,
		CancellationToken cancellationToken)
	{
		List<Result> results = new();

		payloads
			.GroupBy(payload => payload.SubscriptionType)
			.ToList()
			.ForEach(async payloadGroup =>
			{
				var command = payloadGroup.Key switch
				{
					"deal.propertyChange" => new NewInstallationCommand("", "", "", "", ""),
					"deal.deletion" => new NewInstallationCommand("", "", "", "", ""),
					_ => null
				};

				if (command is not null)
				{
					var result = await Sender.Send(command, cancellationToken);
					results.Add(result);
				}
			});

		return Result.FirstFailureOrSuccess(results.ToArray()).IsSucces
			? Ok()
			: BadRequest();
	}
}
