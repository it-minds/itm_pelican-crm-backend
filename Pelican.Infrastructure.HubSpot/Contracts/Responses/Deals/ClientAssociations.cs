using System.Text.Json.Serialization;
using Pelican.Infrastructure.HubSpot.Contracts.Responses.Common;

namespace Pelican.Infrastructure.HubSpot.Contracts.Responses.Deals;

internal sealed class ContactAssociations
{
	[JsonPropertyName("companies")]
	public Associations? Companies { get; set; } = default!;

	[JsonPropertyName("deals")]
	public Associations? Deals { get; set; } = default!;
}

