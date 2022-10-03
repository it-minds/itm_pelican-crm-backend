using Pelican.Application.Abstractions.Messaging;
using Pelican.Domain.Shared;
using RestSharp;

namespace Pelican.Application.HubSpot.Commands.NewInstallation;

internal sealed class NewInstallationCommandHandler : ICommandHandler<NewInstallationCommand>
{
	public async Task<Result> Handle(NewInstallationCommand command, CancellationToken cancellationToken)
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

		return response.IsSuccessful
			? Result.Success()
			: Result.Failure(
				new Error(
					response.StatusCode.ToString(),
					response.ErrorMessage!));
	}
}
