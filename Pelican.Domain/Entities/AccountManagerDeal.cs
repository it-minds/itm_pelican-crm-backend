namespace Pelican.Domain.Entities;
using Pelican.Domain.Primitives;

public class AccountManagerDeal : Entity, ITimeTracked
{
	public Guid AccountManagerId { get; set; }
	public AccountManager? AccountManager { get; set; }
	public Deal? Deal { get; set; }
	public Guid DealId { get; set; }
	public bool IsActive { get; set; }
	public long CreatedAt { get; set; }
	public long? LastUpdatedAt { get; set; }
	public AccountManagerDeal(Guid id) : base(id)
	{
	}
	public AccountManagerDeal()
	{
	}
}
