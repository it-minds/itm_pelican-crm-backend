using System.Text.Json.Serialization;

namespace Pelican.Presentation.Api.Contracts.PipedriveWebHookRequests;
public record UpdateDealCurrent
{
	[JsonPropertyName("stage_id")]
	public int DealStatusId { get; set; }

	[JsonPropertyName("label")]
	public string? DealDescription { get; set; } = string.Empty;

	[JsonPropertyName("title")]
	public string? DealName { get; set; } = string.Empty;

	[JsonPropertyName("last_activity_date")]
	public string? LastContactDate { get; set; } = string.Empty;

	[JsonPropertyName("id")]
	public int DealId { get; set; }
}
