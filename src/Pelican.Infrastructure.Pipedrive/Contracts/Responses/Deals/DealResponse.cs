using System.Text.Json.Serialization;

namespace Pelican.Infrastructure.Pipedrive.Contracts.Responses.Deals;

internal sealed record DealResponse
{
	[JsonPropertyName("data")]
	public DealData Data { get; set; } = new();
}
