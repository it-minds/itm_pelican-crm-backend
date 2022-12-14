namespace Pelican.Domain.Entities;

using HotChocolate;
using Pelican.Domain.Primitives;

public class AccountManagerDeal : Entity, ITimeTracked
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
	public AccountManagerDeal() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

	public bool IsActive { get; set; }

	public Guid AccountManagerId { get; set; }

	public string SourceAccountManagerId { get; set; } = string.Empty;

	public AccountManager AccountManager { get; set; }

	public Deal Deal { get; set; }

	public string SourceDealId { get; set; } = string.Empty;

	public Guid DealId { get; set; }

	public long CreatedAt { get; set; }

	public long? LastUpdatedAt { get; set; }

	[GraphQLIgnore]
	public static AccountManagerDeal Create(Deal deal, AccountManager accountManager)
	{
		return new AccountManagerDeal()
		{
			Deal = deal,
			DealId = deal.Id,
			SourceDealId = deal.SourceId,
			AccountManager = accountManager,
			AccountManagerId = accountManager.Id,
			SourceAccountManagerId = accountManager.SourceId,
			IsActive = true,
		};
	}

	[GraphQLIgnore]
	public void Deactivate()
	{
		IsActive = false;
	}
}
