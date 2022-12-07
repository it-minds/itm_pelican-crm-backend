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
			StartDate = string.IsNullOrWhiteSpace(response.Properties.StartDate) ? null : Convert.ToDateTime(response.Properties.StartDate).Ticks,
			EndDate = string.IsNullOrWhiteSpace(response.Properties.EndDate) ? null : Convert.ToDateTime(response.Properties.EndDate).Ticks,
			LastContactDate = string.IsNullOrWhiteSpace(response.Properties.LastContactDate) ? null : Convert.ToDateTime(response.Properties.LastContactDate).Ticks,
			SourceId = response.Properties.HubSpotObjectId,
			SourceOwnerId = response.Properties.HubSpotOwnerId,
			Name = response.Properties.DealName,
			Description = response.Properties.Description,
			DealStatus = response.Properties.DealStage,
			Source = Sources.HubSpot,
		};

		result.AccountManagerDeals = new List<AccountManagerDeal>()
		{
			new AccountManagerDeal(Guid.NewGuid())
			{
				Deal = result,
				DealId = result.Id,
				SourceDealId = result.SourceId,
				SourceAccountManagerId = result.SourceOwnerId,
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
				SourceDealId = result.SourceId,
				Deal = result,
				SourceContactId = association.Id,
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
				SourceId = association.Id,
				Deals = new List<Deal>() { result },
				Source = Sources.HubSpot,
			})
			.FirstOrDefault();

		result.ClientId = result.Client?.Id;

		return result;
	}
}
