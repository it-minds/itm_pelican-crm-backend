using System.Text.Json.Serialization;
using Pelican.Infrastructure.HubSpot.Contracts.Responses.Common;

namespace Pelican.Infrastructure.HubSpot.Contracts.Responses.Deals;

internal sealed class DealAssociations
{
	[JsonPropertyName("companies")]
	public Associations Companies { get; set; } = new();

	[JsonPropertyName("contacts")]
	public Associations Contacts { get; set; } = new();
}

