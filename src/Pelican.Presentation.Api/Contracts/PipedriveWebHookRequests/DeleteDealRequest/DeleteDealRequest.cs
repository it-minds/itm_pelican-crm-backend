using System.Text.Json.Serialization;

namespace Pelican.Presentation.Api.Contracts.PipedriveWebHookRequests.DeleteDealRequest;
public record DeleteDealRequest
{
	[JsonPropertyName("meta")]
	public MetaProperties MetaProperties { get; set; } = new();
}
