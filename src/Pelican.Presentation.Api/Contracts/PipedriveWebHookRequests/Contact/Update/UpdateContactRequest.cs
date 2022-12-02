using System.Text.Json.Serialization;

namespace Pelican.Presentation.Api.Contracts.PipedriveWebHookRequests.Contact.Update;
public record UpdateContactRequest
{
	[JsonPropertyName("current")]
	public UpdateContactCurrentProperties CurrentProperties { get; set; } = new();

	[JsonPropertyName("meta")]
	public MetaProperties MetaProperties { get; set; } = new();
}
