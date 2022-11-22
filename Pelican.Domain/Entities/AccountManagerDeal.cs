namespace Pelican.Domain.Entities;

using HotChocolate;
using Pelican.Domain.Primitives;

public class AccountManagerDeal : Entity, ITimeTracked
{
	public bool IsActive { get; set; }

	public Guid AccountManagerId { get; set; }

	public string HubSpotAccountManagerId { get; set; }

	public AccountManager AccountManager { get; set; }

	public Deal Deal { get; set; }

	public string HubSpotDealId { get; set; }

	public Guid DealId { get; set; }


	public long CreatedAt { get; set; }

	public long? LastUpdatedAt { get; set; }

	public AccountManagerDeal(Guid id) : base(id) { }

	public AccountManagerDeal() { }

	[GraphQLIgnore]
	public static AccountManagerDeal Create(Deal deal, AccountManager accountManager)
	{
		return new AccountManagerDeal(Guid.NewGuid())
		{
			Deal = deal,
			DealId = deal.Id,
			HubSpotDealId = deal.HubSpotId,
			AccountManager = accountManager,
			AccountManagerId = accountManager.Id,
			HubSpotAccountManagerId = accountManager.HubSpotId,
			IsActive = true,
		};
	}

	[GraphQLIgnore]
	public void Deactivate()
	{
		IsActive = false;
	}
}
