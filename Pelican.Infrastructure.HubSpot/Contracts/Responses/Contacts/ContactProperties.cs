using System.Text.Json.Serialization;

namespace Pelican.Infrastructure.HubSpot.Contracts.Responses.Contacts;

internal sealed class ContactProperties
{
	[JsonPropertyName("company")]
	public string Company { get; set; } = default!;

	[JsonPropertyName("createdate")]
	public string Createdate { get; set; } = default!;

	[JsonPropertyName("email")]
	public string Email { get; set; } = default!;

	[JsonPropertyName("firstname")]
	public string Firstname { get; set; } = default!;

	[JsonPropertyName("hs_object_id")]
	public string HubSpotObjectId { get; set; } = default!;

	[JsonPropertyName("lastmodifieddate")]
	public string LastModifiedDate { get; set; } = default!;

	[JsonPropertyName("lastname")]
	public string Lastname { get; set; } = default!;

	[JsonPropertyName("phone")]
	public string Phone { get; set; } = default!;

	[JsonPropertyName("jobtitle")]
	public string JobTitle { get; set; } = default!;

	[JsonPropertyName("hubspot_owner_id")]
	public string HubSpotOwnerId { get; set; } = default!;
}
