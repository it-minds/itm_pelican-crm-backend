using System.Text.Json.Serialization;

namespace Pelican.Infrastructure.HubSpot.Contracts.Responses.Common;

internal sealed class Paging
{
	[JsonPropertyName("next")]
	public Next Next { get; set; } = new();
}
