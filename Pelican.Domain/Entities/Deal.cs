using Pelican.Domain.Primitives;

namespace Pelican.Domain.Entities;
public class Deal : Entity, ITimeTracked
{
	public ICollection<AccountManagerDeal> AccountManagerDeals { get; set; }
	public Client Client { get; set; }
	public ICollection<DealContactPerson> DealContactPersons { get; set; }
	public decimal Revenue { get; set; }
	public string DealStatus { get; set; }
	public DateOnly EndDate { get; set; }
	public long CreatedAt { get; set; }
	public long? LastUpdatedAt { get; set; }

	public Deal(Guid id, ICollection<AccountManagerDeal> accountManagerDeals,
		Client client,
		ICollection<DealContactPerson> dealContactPersons,
		decimal revenue,
		string dealStatus,
		DateOnly endDate) : base(id)
	{
		AccountManagerDeals = accountManagerDeals;
		Client = client;
		DealContactPersons = dealContactPersons;
		Revenue = revenue;
		DealStatus = dealStatus;
		EndDate = endDate;
	}
}
