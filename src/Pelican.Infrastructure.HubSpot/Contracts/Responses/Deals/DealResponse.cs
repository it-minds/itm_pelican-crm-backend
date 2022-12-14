using System.Text.Json.Serialization;
using Pelican.Infrastructure.HubSpot.Abstractions;

namespace Pelican.Infrastructure.HubSpot.Contracts.Responses.Deals;

internal sealed class DealResponse : HubSpotResponse
{
	[JsonPropertyName("properties")]
	public DealProperties Properties { get; set; } = new();

	[JsonPropertyName("associations")]
	public DealAssociations Associations { get; set; } = new();
}
