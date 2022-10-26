using System.Text.Json.Serialization;

namespace Pelican.Infrastructure.HubSpot.Contracts.Responses.AccountManagers;

internal sealed class OwnersResponse
{
	[JsonPropertyName("results")]
	public IEnumerable<OwnerResponse> Results { get; set; } = Enumerable.Empty<OwnerResponse>();
}
