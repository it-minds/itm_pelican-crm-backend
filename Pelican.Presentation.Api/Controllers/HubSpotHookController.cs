using System.Text;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Pelican.Presentation.Api.Utilities;
using RestSharp;

namespace Pelican.Presentation.Api.Controllers;

[ApiController]
[Route("[controller]")]
[ServiceFilter(typeof(HubSpotHookValidationFilter))]
[EnableCors("HubSpot")]
public sealed class HubSpotHookController : ControllerBase
{
	public HubSpotHookController() { }


	[HttpPost("/NewInstallation")]
	public async Task<IResult> NewInstallation(string code, CancellationToken cancellationToken)
	{
		string url = "https://api.hubapi.com/oauth/v1/token?";
		string clientId = "45c8a55e-7a13-4eae-8b5e-06fa8900b7fa";
		string redirectUrl = "https://eomyft7gbubnxim.m.pipedream.net";
		string client_secret = "1dfc68f9-9ed7-41c8-a45a-ffc32bf274d8";
		string grant_type = "authorization_code";

		var clientUrl = new StringBuilder(url);
		clientUrl.Append("?client_id=" + clientId);
		clientUrl.Append("&code=" + code);
		clientUrl.Append("&redirect_uri=" + redirectUrl);
		clientUrl.Append("&client_secret=" + client_secret);
		clientUrl.Append("&grant_type=" + grant_type);

		var client = new RestClient(clientUrl.ToString());

		//var request = new RestRequest(Method.Post.ToString());
		var request = new RestRequest();
		request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
		request.AddHeader("charset", "utf-8");

		RestResponse response = await client.PostAsync(request, cancellationToken);

		Console.WriteLine(response.Content);

		return Results.Ok();
	}
}
