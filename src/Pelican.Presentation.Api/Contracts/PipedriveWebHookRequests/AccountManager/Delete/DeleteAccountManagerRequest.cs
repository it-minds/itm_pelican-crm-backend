using System.Text.Json.Serialization;

namespace Pelican.Presentation.Api.Contracts.PipedriveWebHookRequests.AccountManager.Delete;
public record DeleteAccountManagerRequest
{
	[JsonPropertyName("meta")]
	public MetaProperties MetaProperties { get; set; } = new();
}
