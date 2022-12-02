using System.Text.Json.Serialization;

namespace Pelican.Presentation.Api.Contracts.PipedriveWebHookRequests.Client.Update;
public record UpdateClientCurrentProperties
{
	[JsonPropertyName("name")]
	public string ClientName { get; set; } = string.Empty;

	[JsonPropertyName("address_locality")]
	public string? OfficeLocation { get; set; } = string.Empty;

}
