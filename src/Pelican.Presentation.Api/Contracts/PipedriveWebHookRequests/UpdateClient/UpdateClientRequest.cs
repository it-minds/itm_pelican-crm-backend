using System.Text.Json.Serialization;

namespace Pelican.Presentation.Api.Contracts.PipedriveWebHookRequests.UpdateClient;
public record UpdateClientRequest
{
	[JsonPropertyName("current")]
	public UpdateClientCurrentProperties CurrentProperties { get; set; } = new();

	[JsonPropertyName("meta")]
	public MetaProperties MetaProperties { get; set; } = new();
}
