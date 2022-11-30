using System.Text.Json.Serialization;

namespace Pelican.Presentation.Api.Contracts.PipedriveWebHookRequests.AccountManager.DeleteAccountManager;
public record DeleteAccountManagerRequest
{
	[JsonPropertyName("meta")]
	public MetaProperties MetaProperties { get; set; } = new();
}
