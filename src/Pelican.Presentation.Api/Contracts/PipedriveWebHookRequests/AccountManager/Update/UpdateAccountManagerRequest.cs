using System.Text.Json.Serialization;

namespace Pelican.Presentation.Api.Contracts.PipedriveWebHookRequests.AccountManager.Update;
public record UpdateAccountManagerRequest
{
	[JsonPropertyName("current/0")]
	public UpdateAccountManagerCurrentProperties CurrentProperties { get; set; } = new();

	[JsonPropertyName("meta")]
	public MetaProperties MetaProperties { get; set; } = new();
}
