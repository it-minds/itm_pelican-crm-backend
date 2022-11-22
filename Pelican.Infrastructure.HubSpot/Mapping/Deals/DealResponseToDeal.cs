using Pelican.Domain;
using Pelican.Domain.Entities;
using Pelican.Infrastructure.HubSpot.Contracts.Responses.Deals;

namespace Pelican.Infrastructure.HubSpot.Mapping.Deals;

internal static class DealResponseToDeal
{
	internal static Deal ToDeal(this DealResponse response)
	{
		if (string.IsNullOrWhiteSpace(response.Properties.HubSpotObjectId))
		{
			throw new ArgumentNullException(nameof(response));
		}

		Deal result = new(Guid.NewGuid())
		{
			StartDate = string.IsNullOrWhiteSpace(response.Properties.StartDate) ? null : DateTime.Parse(response.Properties.StartDate),
			EndDate = string.IsNullOrWhiteSpace(response.Properties.EndDate) ? null : DateTime.Parse(response.Properties.EndDate),
			LastContactDate = string.IsNullOrWhiteSpace(response.Properties.LastContactDate) ? null : DateTime.Parse(response.Properties.LastContactDate),
			DealStatus = response.Properties.DealStage.Length > StringLengths.DealStatus ? response.Properties.DealStage.Substring(0, StringLengths.DealStatus - 3) + ("...") : response.Properties.DealStage,
			HubSpotId = response.Properties.HubSpotObjectId,
			HubSpotOwnerId = response.Properties.HubSpotOwnerId,
			Name = response.Properties.DealName.Length > StringLengths.DealName ? response.Properties.DealName.Substring(0, StringLengths.DealName - 3) + ("...") : response.Properties.DealName,
			Description = response.Properties.Description.Length > StringLengths.DealDescription ? response.Properties.Description.Substring(0, StringLengths.DealDescription - 3) + ("...") : response.Properties.Description
		};


		result.AccountManagerDeals = new List<AccountManagerDeal>()
		{
			new AccountManagerDeal(Guid.NewGuid())
			{
				Deal = result,
				DealId = result.Id,
				HubSpotDealId = result.HubSpotId,
				HubSpotAccountManagerId = result.HubSpotOwnerId,
				IsActive = true,
			}
		};

		result.DealContacts = response
			.Associations
			.Contacts
			.AssociationList
			.Where(association => association.Type == "deal_to_contact")
			.Select(association => new DealContact(Guid.NewGuid())
			{
				DealId = result.Id,
				HubSpotDealId = result.HubSpotId,
				Deal = result,
				HubSpotContactId = association.Id,
				IsActive = true,
			})
			.ToList();

		result.Client = response
			.Associations
			.Companies
			.AssociationList
			.Where(association => association.Type == "deal_to_company")
			.Select(association => new Client(Guid.NewGuid())
			{
				HubSpotId = association.Id,
				Deals = new List<Deal>() { result },
			})
			.FirstOrDefault();

		result.ClientId = result.Client?.Id;

		return result;
	}
}
