using System.Text.Json.Serialization;

namespace Pelican.Infrastructure.HubSpot.Contracts.Responses.Deals;

internal sealed class DealProperties
{
	[JsonPropertyName("amount")]
	public string Amount { get; set; } = string.Empty;

	[JsonPropertyName("closedate")]
	public DateTime CloseDate { get; set; }

	[JsonPropertyName("createdate")]
	public string CreateDate { get; set; } = string.Empty;

	[JsonPropertyName("dealname")]
	public string Dealname { get; set; } = string.Empty;

	[JsonPropertyName("dealstage")]
	public string Dealstage { get; set; } = string.Empty;

	[JsonPropertyName("hs_lastmodifieddate")]
	public string LastModifiedDate { get; set; } = string.Empty;

	[JsonPropertyName("hs_object_id")]
	public string HubSpotObjectId { get; set; } = string.Empty;

	[JsonPropertyName("hubspot_owner_id")]
	public string HubSpotOwnerId { get; set; } = string.Empty;
}
