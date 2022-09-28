namespace Pelican.Domain.Entities;
using Pelican.Domain.Primitives;

public class AccountManagerDeal : Entity, ITimeTracked
{
	public int AccountManagerId { get; set; }
	public int DealId { get; set; }
	public bool IsActive { get; set; }
	public long CreatedAt { get; set; }
	public long? LastUpdatedAt { get; set; }

	public AccountManagerDeal(Guid id, int accountManagerId,
		int dealId,
		bool isActive) : base(id)
	{
		AccountManagerId = accountManagerId;
		DealId = dealId;
		IsActive = isActive;
	}
}
