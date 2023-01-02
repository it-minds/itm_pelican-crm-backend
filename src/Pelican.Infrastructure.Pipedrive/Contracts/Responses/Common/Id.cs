using System.Text.Json.Serialization;

namespace Pelican.Infrastructure.Pipedrive.Contracts.Responses.Common;

internal record Id
{
	[JsonPropertyName("value")]
	public long Value { get; set; }
}
