using System.Text.Json.Serialization;

namespace Pelican.Infrastructure.HubSpot.Contracts.Responses;

internal sealed class OwnerResponse
{
	[JsonPropertyName("id")]
	public string Id { get; set; } = default!;

	[JsonPropertyName("email")]
	public string Email { get; set; } = default!;

	[JsonPropertyName("firstName")]
	public string Firstname { get; set; } = default!;

	[JsonPropertyName("lastName")]
	public string Lastname { get; set; } = default!;

	[JsonPropertyName("userId")]
	public long UserId { get; set; }
}
