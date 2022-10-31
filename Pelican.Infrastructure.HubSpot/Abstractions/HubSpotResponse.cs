using System.Text.Json.Serialization;

namespace Pelican.Infrastructure.HubSpot.Abstractions;

internal abstract class HubSpotResponse
{
	[JsonPropertyName("id")]
	public string Id { get; set; } = string.Empty;

	[JsonPropertyName("createdAt")]
	public string CreatedAt { get; set; } = string.Empty;

	[JsonPropertyName("updatedAt")]
	public string UpdatedAt { get; set; } = string.Empty;

	[JsonPropertyName("archived")]
	public bool Archived { get; set; }
}
