using System.Text.Json.Serialization;

namespace Pelican.Infrastructure.HubSpot.Contracts.Responses.Common;

internal sealed class Association
{
	[JsonPropertyName("id")]
	public string Id { get; set; } = default!;

	[JsonPropertyName("type")]
	public string Type { get; set; } = default!;
}
