using System.Text.Json.Serialization;

namespace Pelican.Presentation.Api.Contracts.PipedriveWebHookRequests.DeleteDeal;
public record DeleteDealRequest
{
	[JsonPropertyName("meta")]
	public MetaProperties MetaProperties { get; set; } = new();
}
