using System.Text.Json.Serialization;

namespace Pelican.Presentation.Api.Contracts;
public record WebHookRequest
{
	[JsonPropertyName("objectId")]
	public long ObjectId { get; set; }

	[JsonPropertyName("propertyName")]
	public string PropertyName { get; set; } = string.Empty;

	[JsonPropertyName("propertyValue")]
	public string PropertyValue { get; set; } = string.Empty;

	[JsonPropertyName("attemptNumber")]
	public int AttemptNumber { get; set; }

	[JsonPropertyName("subscriptionType")]
	public string SubscriptionType { get; set; } = string.Empty;

	[JsonPropertyName("portalId")]
	public long PortalId { get; set; }
}
