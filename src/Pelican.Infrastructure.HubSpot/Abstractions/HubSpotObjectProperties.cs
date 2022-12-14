using System.Text.Json.Serialization;

namespace Pelican.Infrastructure.HubSpot.Abstractions;

internal abstract class HubSpotObjectProperties
{
	[JsonPropertyName("createdate")]
	public DateTime Createdate { get; set; }

	[JsonPropertyName("hs_object_id")]
	public string HubSpotObjectId { get; set; } = String.Empty;

	[JsonPropertyName("occurredAt")]
	public long ChangedUnixTime { get; set; }
}
