using System.Text.Json.Serialization;
using Pelican.Presentation.Api.Contracts.PipedriveWebHookRequests.UpdateDealRequest;

namespace Pelican.Presentation.Api.Contracts.PipedriveWebHookRequests.DeleteDealRequest;
public record DeleteDealResponse
{
	[JsonPropertyName("meta")]
	public DeleteDealMeta MetaProperties { get; set; } = new();
}
