using System.Text.Json.Serialization;

namespace Pelican.Presentation.Api.Contracts.PipedriveWebHookRequests;
public record UpdateDealResponse
{
	[JsonPropertyName("current")]
	public UpdateDealCurrent CurrentProperties { get; set; } = new();

	[JsonPropertyName("meta")]
	public UpdateDealMeta MetaProperties { get; set; } = new();
}
