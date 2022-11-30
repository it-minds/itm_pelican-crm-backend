using System.Text.Json.Serialization;

namespace Pelican.Presentation.Api.Contracts.PipedriveWebHookRequests.UpdateDealRequest;
public record UpdateDealResponse
{
	[JsonPropertyName("current")]
	public UpdateDealCurrent CurrentProperties { get; set; } = new();

	[JsonPropertyName("meta")]
	public DeleteDealMeta MetaProperties { get; set; } = new();
}
