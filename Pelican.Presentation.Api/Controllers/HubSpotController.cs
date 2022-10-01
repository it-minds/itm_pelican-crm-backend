using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Pelican.Application.HubSpot.Command.NewInstallation;
using Pelican.Domain.Shared;
using Pelican.Infrastructure.HubSpot;
using Pelican.Presentation.Api.Abstractions;

namespace Pelican.Presentation.Api.Controllers;

[ApiController]
[Route("[controller]")]
//[ServiceFilter(typeof(HubSpotHookValidationFilter))]
//[EnableCors("HubSpot")]
public sealed class HubSpotController : ApiController
{
	private readonly IOptions<HubSpotSettings> _hubSpotSettings;

	public HubSpotController(ISender sender, IOptions<HubSpotSettings> hubSpotSettings) : base(sender)
	{
		_hubSpotSettings = hubSpotSettings;
	}


	[HttpPost("/NewInstallation")]
	public async Task<IActionResult> NewInstallation(string code, CancellationToken cancellationToken)
	{
		if (_hubSpotSettings is null) throw new NullReferenceException();

		NewInstallationCommand newInstallation = new(
			code,
			_hubSpotSettings.Value.BaseUrl ?? throw new NullReferenceException(),
			_hubSpotSettings.Value.App?.ClientId ?? throw new NullReferenceException(),
			"https://eomyft7gbubnxim.m.pipedream.net",
			_hubSpotSettings.Value.App.ClientSecret ?? throw new NullReferenceException());

		Result<Unit> result = await Sender.Send(newInstallation, cancellationToken);
		Console.WriteLine(result);

		return result.IsSucces
			? Ok()
			: BadRequest();
	}
}
