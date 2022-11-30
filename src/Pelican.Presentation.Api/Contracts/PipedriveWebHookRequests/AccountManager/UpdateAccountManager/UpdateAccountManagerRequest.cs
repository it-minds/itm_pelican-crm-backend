using System.Text.Json.Serialization;

namespace Pelican.Presentation.Api.Contracts.PipedriveWebHookRequests.AccountManager.UpdateAccountManager;
public record UpdateAccountManagerRequest
{
	[JsonPropertyName("current")]
	public UpdateAccountManagerCurrentProperties CurrentProperties { get; set; } = new();

	[JsonPropertyName("meta")]
	public MetaProperties MetaProperties { get; set; } = new();
}
