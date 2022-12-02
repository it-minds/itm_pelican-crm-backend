using System.Text.Json.Serialization;

namespace Pelican.Presentation.Api.Contracts.PipedriveWebHookRequests;
public record ContactItem
{
	[JsonPropertyName("value")]
	public string Value { get; set; } = string.Empty;
	[JsonPropertyName("primary")]
	public bool Primary { get; set; }
	[JsonPropertyName("label")]
	public string Label { get; set; } = string.Empty;
}
