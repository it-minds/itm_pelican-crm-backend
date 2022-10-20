using System.Text.Json.Serialization;

namespace Pelican.Infrastructure.HubSpot.Contracts.Responses.Deals;

internal sealed class DealProperties
{
	[JsonPropertyName("amount")]
	public string Amount { get; set; } = default!;

	[JsonPropertyName("closedate")]
	public DateTime? CloseDate { get; set; } = default!;

	[JsonPropertyName("createdate")]
	public string CreateDate { get; set; } = default!;

	[JsonPropertyName("dealname")]
	public string Dealname { get; set; } = default!;

	[JsonPropertyName("dealstage")]
	public string Dealstage { get; set; } = default!;

	[JsonPropertyName("hs_lastmodifieddate")]
	public string LastModifiedDate { get; set; } = default!;

	[JsonPropertyName("hs_object_id")]
	public string HubSpotObjectId { get; set; } = default!;

	[JsonPropertyName("hubspot_owner_id")]
	public string HubspotOwnerId { get; set; } = default!;
}
