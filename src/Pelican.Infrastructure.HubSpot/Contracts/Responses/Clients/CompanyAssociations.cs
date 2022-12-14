using System.Text.Json.Serialization;
using Pelican.Infrastructure.HubSpot.Contracts.Responses.Common;

namespace Pelican.Infrastructure.HubSpot.Contracts.Responses.Clients;

internal sealed class CompanyAssociations
{
	[JsonPropertyName("deals")]
	public Associations Deals { get; set; } = new();

	[JsonPropertyName("contacts")]
	public Associations Contacts { get; set; } = new();
}
