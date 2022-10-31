using System.Text.Json.Serialization;
using Pelican.Infrastructure.HubSpot.Abstractions;

namespace Pelican.Infrastructure.HubSpot.Contracts.Responses.Clients;

internal sealed class CompanyProperties : HubSpotObjectProperties
{
	[JsonPropertyName("domain")]
	public string Domain { get; set; } = string.Empty;

	[JsonPropertyName("hs_lastmodifieddate")]
	public DateTime LastModifiedDate { get; set; }

	[JsonPropertyName("name")]
	public string Name { get; set; } = string.Empty;

	[JsonPropertyName("industry")]
	public string Industry { get; set; } = string.Empty;

	[JsonPropertyName("city")]
	public string City { get; set; } = string.Empty;
}
