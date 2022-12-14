using System.Text.Json.Serialization;

namespace Pelican.Presentation.Api.Contracts.PipedriveWebHookRequests.Deal.Update;
public record UpdateDealRequest
{
	[JsonPropertyName("current")]
	public UpdateDealCurrentProperties CurrentProperties { get; set; } = new();

	[JsonPropertyName("meta")]
	public MetaProperties MetaProperties { get; set; } = new();
}
