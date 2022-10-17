using System.Text.Json.Serialization;

namespace Pelican.Infrastructure.HubSpot.Contracts.Responses.AccountManagers;

internal sealed class OwnersResponse
{
	[JsonPropertyName("results")]
	public OwnerResponse[] Results { get; set; } = default!;
}
