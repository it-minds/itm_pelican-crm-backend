using System.Text.Json.Serialization;
using Pelican.Infrastructure.Pipedrive.Contracts.Responses.Common;

namespace Pelican.Infrastructure.Pipedrive.Contracts.Responses.Deals;

internal record DealData : DataProperties
{
	[JsonPropertyName("title")]
	public string Title { get; set; } = string.Empty;

	[JsonPropertyName("stage_id")]
	public int StageId { get; set; }

	[JsonPropertyName("last_activity_date")]
	public string? LastActivityDate { get; set; }

	[JsonPropertyName("person_id")]
	public Id? PersonId { get; set; }

	[JsonPropertyName("org_id")]
	public Id? OrgId { get; set; }
}
