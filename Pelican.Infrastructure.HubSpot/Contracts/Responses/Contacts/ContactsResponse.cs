using System.Text.Json.Serialization;

namespace Pelican.Infrastructure.HubSpot.Contracts.Responses.Contacts;

internal sealed class ContactsResponse
{
	[JsonPropertyName("results")]
	public IEnumerable<ContactResponse> Results { get; set; } = default!;
}
