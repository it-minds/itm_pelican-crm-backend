using System.Text.Json.Serialization;

namespace Pelican.Presentation.Api.Contracts.PipedriveWebHookRequests.UpdateDeal;
public record UpdateDealCurrentProperties
{
	[JsonPropertyName("stage_id")]
	public int DealStatusId { get; set; }

	[JsonPropertyName("label")]
	public string? DealDescription { get; set; } = string.Empty;

	[JsonPropertyName("title")]
	public string? DealName { get; set; } = string.Empty;

	[JsonPropertyName("last_activity_date")]
	public string? LastContactDate { get; set; } = string.Empty;
}
