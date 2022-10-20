using System.Text.Json.Serialization;

namespace Pelican.Infrastructure.HubSpot.Contracts.Responses.Clients;

internal sealed class CompaniesResponse
{
	[JsonPropertyName("results")]
	public IEnumerable<CompanyResponse> Results { get; set; } = default!;
}
