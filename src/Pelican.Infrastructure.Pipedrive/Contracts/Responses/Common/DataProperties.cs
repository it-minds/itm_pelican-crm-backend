using System.Text.Json.Serialization;

namespace Pelican.Infrastructure.Pipedrive.Contracts.Responses.Common;

internal abstract record DataProperties
{
	[JsonPropertyName("id")]
	public long Id { get; set; }

	[JsonPropertyName("user_id")]
	public Id? UserId { get; set; }
}
