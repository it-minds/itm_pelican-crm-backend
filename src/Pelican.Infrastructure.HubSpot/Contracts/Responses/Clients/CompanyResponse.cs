using System.Text.Json.Serialization;
using Pelican.Infrastructure.HubSpot.Abstractions;

namespace Pelican.Infrastructure.HubSpot.Contracts.Responses.Clients;

internal sealed class CompanyResponse : HubSpotResponse
{
	[JsonPropertyName("properties")]
	public CompanyProperties Properties { get; set; } = new();

	[JsonPropertyName("associations")]
	public CompanyAssociations Associations { get; set; } = new();
}
