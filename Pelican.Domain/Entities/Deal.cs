using Pelican.Domain.Primitives;

namespace Pelican.Domain.Entities;
public class Deal : Entity, ITimeTracked
{
	public ICollection<AccountManagerDeal> AccountManagerDeals { get; set; }
	public Client Client { get; set; }
	public Guid ClientId { get; set; }
	public ICollection<DealContact> DealContacts { get; set; }
	public decimal? Revenue { get; set; }
	public string DealStatus { get; set; }
	public DateOnly EndDate { get; set; }
	public long CreatedAt { get; set; }
	public long? LastUpdatedAt { get; set; }

	public Deal(Guid id, ICollection<AccountManagerDeal> accountManagerDeals,
		Client client,
		ICollection<DealContact> dealContacts,
		decimal? revenue,
		string dealStatus,
		DateOnly endDate,
		Guid clientId) : base(id)
	{
		AccountManagerDeals = accountManagerDeals;
		Client = client;
		DealContacts = dealContacts;
		Revenue = revenue;
		DealStatus = dealStatus;
		EndDate = endDate;
		ClientId = clientId;
	}
}
