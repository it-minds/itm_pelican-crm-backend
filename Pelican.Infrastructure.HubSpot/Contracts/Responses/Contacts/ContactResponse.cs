using System.Text.Json.Serialization;
using Pelican.Infrastructure.HubSpot.Abstractions;
using Pelican.Infrastructure.HubSpot.Contracts.Responses.Deals;

namespace Pelican.Infrastructure.HubSpot.Contracts.Responses.Contacts;

internal sealed class ContactResponse : HubSpotResponse
{
	[JsonPropertyName("properties")]
	public ContactProperties Properties { get; set; } = default!;

	[JsonPropertyName("associations")]
	public ContactAssociations? Associations { get; set; } = default!;
}
