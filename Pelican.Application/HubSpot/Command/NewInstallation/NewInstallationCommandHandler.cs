using MediatR;
using Pelican.Application.Abstractions.Messaging;
using Pelican.Domain.Shared;
using RestSharp;

namespace Pelican.Application.HubSpot.Command.NewInstallation;

internal sealed class NewInstallationCommandHandler : ICommandHandler<NewInstallationCommand, Unit>
{
	public async Task<Result<Unit>> Handle(NewInstallationCommand command, CancellationToken cancellationToken)
	{
		var client = new RestClient(command.BaseUrl);

		var request = new RestRequest("oauth/v1/token")
			.AddHeader("Content-Type", "application/x-www-form-urlencoded")
			.AddHeader("charset", "utf-8")
			.AddQueryParameter("client_id", command.ClientId, false)
			.AddQueryParameter("code", command.Code, false)
			.AddQueryParameter("redirect_uri", command.RedirectUrl, false)
			.AddQueryParameter("client_secret", command.ClientSecret, false)
			.AddQueryParameter("grant_type", "authorization_code", false);

		RestResponse response = await client.ExecutePostAsync(request, cancellationToken);

		Console.WriteLine(response.Content);

		return Unit.Value;
	}
}
