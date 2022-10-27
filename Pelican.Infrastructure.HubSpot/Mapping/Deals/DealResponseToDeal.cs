using Pelican.Domain.Entities;
using Pelican.Infrastructure.HubSpot.Contracts.Responses.Deals;

namespace Pelican.Infrastructure.HubSpot.Mapping.Deals;

internal static class DealResponseToDeal
{
	internal static Deal ToDeal(this DealResponse response)
	{
		if (string.IsNullOrWhiteSpace(response.Properties.HubSpotObjectId)
			|| string.IsNullOrWhiteSpace(response.Properties.HubSpotOwnerId))
		{
			throw new ArgumentNullException(nameof(response));
		}

		bool hasAmount = !string.IsNullOrWhiteSpace(response.Properties.Amount);
		bool hasDecimalAmount = decimal.TryParse(response.Properties.Amount, out decimal decimalAmount);

		if (hasAmount
			&& !hasDecimalAmount)
		{
			throw new InvalidOperationException($"nameof(response.Properties.Amount) cant be parsed to decimal");
		}

		Deal result = new(Guid.NewGuid())
		{
			EndDate = response.Properties.CloseDate,
			DealStatus = response.Properties.Dealstage,
			HubSpotId = response.Properties.HubSpotObjectId,
			Revenue = hasAmount ? decimalAmount : null,
			HubSpotOwnerId = response.Properties.HubSpotOwnerId,
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
