﻿using Pelican.Domain.Entities;
using Pelican.Infrastructure.HubSpot.Contracts.Responses.Deals;

namespace Pelican.Infrastructure.HubSpot.Mapping;

internal static class DealResponseToDeal
{
	internal static Deal ToDeal(this DealResponse response)
	{
		Deal result = new(Guid.NewGuid())
		{
			EndDate = response.Properties.CloseDate,
			DealStatus = response.Properties.Dealstage,
			HubSpotId = response.Properties.HubSpotObjectId,
			Revenue = response.Properties.Amount is not "" ? Convert.ToDecimal(response.Properties.Amount) : null,
			HubSpotOwnerId = response.Properties.HubspotOwnerId,
			Client = response
				.Associations?
				.Companies?
				.AssociationList
				.Where(company => company.Type == "deal_to_company")?
				.Select(company => new Client(Guid.NewGuid())
				{
					HubSpotId = company.Id,
				})
				.FirstOrDefault(),
		};

		result.DealContacts = response
			.Associations?
			.Contacts?
			.AssociationList
			.Where(contact => contact.Type == "deal_to_contact")
			.Select(contact => new DealContact(Guid.NewGuid())
			{
				DealId = result.Id,
				HubSpotDealId = result.HubSpotId,
				Deal = result,
				HubSpotContactId = contact.Id,
				IsActive = true,
			})
			.ToList()
			?? new List<DealContact>();

		result.ClientId = result.Client?.Id;

		return result;
	}
}
