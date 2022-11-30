using System.Text.Json.Serialization;

namespace Pelican.Presentation.Api.Contracts.PipedriveWebHookRequests;
public record MetaProperties
{
	[JsonPropertyName("company_id")]
	public int SupplierPipedriveId { get; set; }

	[JsonPropertyName("user_id")]
	public int UserId { get; set; }

	[JsonPropertyName("id")]
	public int ObjectId { get; set; }
}
