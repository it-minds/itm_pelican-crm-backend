namespace Pelican.Application.HubSpot.Dtos;
public sealed class RefreshAccessTokens
{
	public string RefreshToken { get; init; } = default!;
	public string AccessToken { get; init; } = default!;
}
