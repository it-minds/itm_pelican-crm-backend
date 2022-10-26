using System.Text.Json.Serialization;

namespace Pelican.Infrastructure.HubSpot.Contracts.Responses.Common;

internal sealed class Associations
{
	[JsonPropertyName("results")]
	public IEnumerable<Association> AssociationList { get; set; } = Enumerable.Empty<Association>();
}
