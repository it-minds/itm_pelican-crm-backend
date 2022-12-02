using System.Text.Json.Serialization;

namespace Pelican.Presentation.Api.Contracts.PipedriveWebHookRequests.Client.Delete;
public record DeleteClientRequest
{
	[JsonPropertyName("meta")]
	public MetaProperties MetaProperties { get; set; } = new();
}
