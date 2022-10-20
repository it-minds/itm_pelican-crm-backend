using System.Text.Json.Serialization;

namespace Pelican.Infrastructure.HubSpot.Contracts.Responses.Deals;

internal sealed class DealsResponse
{
	[JsonPropertyName("results")]
	public IEnumerable<DealResponse> Results { get; set; } = default!;
}
