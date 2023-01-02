using System.Text.Json.Serialization;
using Pelican.Infrastructure.HubSpot.Abstractions;

namespace Pelican.Infrastructure.HubSpot.Contracts.Responses.Contacts;

internal sealed class ContactProperties : HubSpotObjectProperties
{
	[JsonPropertyName("email")]
	public string Email { get; set; } = string.Empty;

	[JsonPropertyName("firstname")]
	public string FirstName { get; set; } = string.Empty;

	[JsonPropertyName("lastmodifieddate")]
	public string LastModifiedDate { get; set; } = string.Empty;

	[JsonPropertyName("lastname")]
	public string LastName { get; set; } = string.Empty;

	[JsonPropertyName("phone")]
	public string Phone { get; set; } = string.Empty;

	[JsonPropertyName("jobtitle")]
	public string JobTitle { get; set; } = string.Empty;

	[JsonPropertyName("hubspot_owner_id")]
	public string HubSpotOwnerId { get; set; } = string.Empty;
}
