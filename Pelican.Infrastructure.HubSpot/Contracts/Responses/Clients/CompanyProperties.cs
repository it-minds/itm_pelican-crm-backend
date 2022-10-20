using System.Text.Json.Serialization;

namespace Pelican.Infrastructure.HubSpot.Contracts.Responses.Clients;

internal sealed class CompanyProperties
{
	[JsonPropertyName("createdate")]
	public DateTime Createdate { get; set; }

	[JsonPropertyName("domain")]
	public string Domain { get; set; } = default!;

	[JsonPropertyName("hs_lastmodifieddate")]
	public DateTime LastModifiedDate { get; set; }

	[JsonPropertyName("hs_object_id")]
	public string HubSpotObjectId { get; set; } = default!;

	[JsonPropertyName("name")]
	public string Name { get; set; } = default!;

	[JsonPropertyName("industry")]
	public string Industry { get; set; } = default!;

	[JsonPropertyName("city")]
	public string City { get; set; } = default!;
}
