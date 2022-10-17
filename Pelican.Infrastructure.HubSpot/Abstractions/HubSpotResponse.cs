using System.Text.Json.Serialization;

namespace Pelican.Infrastructure.HubSpot.Abstractions;

public class HubSpotResponse
{
	[JsonPropertyName("id")]
	public string Id { get; set; } = default!;

	[JsonPropertyName("createdAt")]
	public string CreatedAt { get; set; } = default!;

	[JsonPropertyName("updatedAt")]
	public string UpdatedAt { get; set; } = default!;

	[JsonPropertyName("archived")]
	public bool Archived { get; set; }
}
