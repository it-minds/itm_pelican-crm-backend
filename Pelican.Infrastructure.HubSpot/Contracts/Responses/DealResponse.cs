using System.Text.Json.Serialization;

namespace Pelican.Infrastructure.HubSpot.Contracts.Responses;
internal sealed class DealResponse
{
	[JsonPropertyName("id")]
	public string Id { get; set; }

	[JsonPropertyName("properties")]
	public DealProperties dealResponse { get; set; }

	[JsonPropertyName("createdAt")]
	public string CreatedAt { get; set; }

	[JsonPropertyName("updatedAt")]
	public string UpdatedAt { get; set; }

	[JsonPropertyName("archived")]
	public bool Archived { get; set; }
}

internal sealed class DealProperties
{
	[JsonPropertyName("amount")]
	public string Amount { get; set; }

	[JsonPropertyName("closedate")]
	public string CloseDate { get; set; }

	[JsonPropertyName("createdate")]
	public string CreateDate { get; set; }

	[JsonPropertyName("dealname")]
	public string Dealname { get; set; }

	[JsonPropertyName("dealstage")]
	public string Dealstage { get; set; }

	[JsonPropertyName("hs_lastmodifieddate")]
	public string LastModifiedDate { get; set; }

	[JsonPropertyName("hs_object_id")]
	public string ObjectId { get; set; }

	[JsonPropertyName("pipeline")]
	public string Pipeline { get; set; }
}
