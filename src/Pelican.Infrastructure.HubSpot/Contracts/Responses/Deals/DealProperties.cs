using System.Text.Json.Serialization;
using Pelican.Infrastructure.HubSpot.Abstractions;

namespace Pelican.Infrastructure.HubSpot.Contracts.Responses.Deals;

internal sealed class DealProperties : HubSpotObjectProperties
{
	[JsonPropertyName("enddate")]
	public string EndDate { get; set; } = string.Empty;

	[JsonPropertyName("startdate")]
	public string StartDate { get; set; } = string.Empty;

	[JsonPropertyName("notes_last_contacted")]
	public string LastContactDate { get; set; } = string.Empty;

	[JsonPropertyName("dealname")]
	public string DealName { get; set; } = string.Empty;

	[JsonPropertyName("dealstage")]
	public string DealStage { get; set; } = string.Empty;

	[JsonPropertyName("hs_lastmodifieddate")]
	public string LastModifiedDate { get; set; } = string.Empty;

	[JsonPropertyName("hubspot_owner_id")]
	public string HubSpotOwnerId { get; set; } = string.Empty;

	[JsonPropertyName("description")]
	public string Description { get; set; } = string.Empty;

}
