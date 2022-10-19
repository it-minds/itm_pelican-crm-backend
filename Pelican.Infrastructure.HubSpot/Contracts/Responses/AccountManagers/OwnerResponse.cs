using System.Text.Json.Serialization;
using Pelican.Infrastructure.HubSpot.Abstractions;

namespace Pelican.Infrastructure.HubSpot.Contracts.Responses.AccountManagers;

internal sealed class OwnerResponse : HubSpotResponse
{
	[JsonPropertyName("email")]
	public string Email { get; set; } = default!;

	[JsonPropertyName("firstName")]
	public string Firstname { get; set; } = default!;

	[JsonPropertyName("lastName")]
	public string Lastname { get; set; } = default!;

	[JsonPropertyName("userId")]
	public long UserId { get; set; }
}
