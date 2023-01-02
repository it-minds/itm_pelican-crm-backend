using System.Text.Json.Serialization;
using Pelican.Infrastructure.HubSpot.Contracts.Responses.Common;

namespace Pelican.Infrastructure.HubSpot.Contracts.Responses.Contacts;

internal sealed class ContactAssociations
{
	[JsonPropertyName("companies")]
	public Associations Companies { get; set; } = new();

	[JsonPropertyName("deals")]
	public Associations Deals { get; set; } = new();
}

