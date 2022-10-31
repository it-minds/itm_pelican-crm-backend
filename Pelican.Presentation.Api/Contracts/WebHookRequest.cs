using System.Text.Json.Serialization;

namespace Pelican.Presentation.Api.Contracts;
public class WebHookRequest
{
	[JsonPropertyName("objectId")]
	public long ObjectId { get; set; }
	[JsonPropertyName("propertyName")]
	public string? PropertyName { get; set; }
	[JsonPropertyName("propertyValue")]
	public string? PropertyValue { get; set; }
	[JsonPropertyName("attemptNumber")]
	public int AttemptNumber { get; set; }
	[JsonPropertyName("subscriptionType")]
	public string? SubscriptionType { get; set; }
	[JsonPropertyName("portalId")]
	public long PortalId { get; set; }
}
