using Pelican.Domain.Entities;
using Pelican.Infrastructure.HubSpot.Contracts.Responses.Clients;

namespace Pelican.Infrastructure.HubSpot.Mapping;

internal static class CompanyResponseToClient
{
	internal static Client ToClient(this CompanyResponse response)
		=> new()
		{
			Name = response.Properties.Name,
			HubSpotId = response.Properties.HubSpotObjectId,
			OfficeLocation = response.Properties.City,
			Segment = response.Properties.Industry,
		};
}
