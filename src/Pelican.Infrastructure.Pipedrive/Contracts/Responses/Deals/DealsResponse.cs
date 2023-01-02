using System.Text.Json.Serialization;

namespace Pelican.Infrastructure.Pipedrive.Contracts.Responses.Deals;

internal sealed record DealsResponse
{
	[JsonPropertyName("data")]
	public List<DealData> Data { get; set; } = new();
}
