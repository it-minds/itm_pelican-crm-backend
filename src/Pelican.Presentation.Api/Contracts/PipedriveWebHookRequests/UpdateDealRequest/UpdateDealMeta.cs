using System.Text.Json.Serialization;

namespace Pelican.Presentation.Api.Contracts.PipedriveWebHookRequests.UpdateDealRequest;
public record UpdateDealMeta
{
	[JsonPropertyName("company_id")]
	public int SupplierPipedriveId { get; set; }

	[JsonPropertyName("action")]
	public string SubscriptionAction { get; set; } = string.Empty;

	[JsonPropertyName("object")]
	public string SubscriptionObject { get; set; } = string.Empty;

	[JsonPropertyName("user_id")]
	public int UserId { get; set; }
}
