using System.Text.Json.Serialization;
using Pelican.Infrastructure.HubSpot.Abstractions;

namespace Pelican.Infrastructure.HubSpot.Contracts.Responses.Contacts;

internal sealed class ContactResponse : HubSpotResponse
{
	[JsonPropertyName("properties")]
	public ContactProperties Properties { get; set; } = new();

	[JsonPropertyName("associations")]
	public ContactAssociations Associations { get; set; } = new();
}
