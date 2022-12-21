using System.Text.Json.Serialization;

namespace Pelican.Infrastructure.HubSpot.Contracts.Responses.Common;

internal sealed class Next
{
	[JsonPropertyName("after")]
	public string After { get; set; } = string.Empty;
}
