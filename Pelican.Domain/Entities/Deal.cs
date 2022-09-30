using System.Collections.ObjectModel;
using Pelican.Domain.Primitives;

namespace Pelican.Domain.Entities;
public class Deal : Entity, ITimeTracked
{
	public ICollection<AccountManagerDeal> AccountManagerDeals { get; set; }
	public Client? Client { get; set; }
	public Guid ClientId { get; set; }
	public ICollection<DealContact> DealContacts { get; set; }
	public decimal? Revenue { get; set; }
	public string DealStatus { get; set; }
	public DateTime EndDate { get; set; }
	public long CreatedAt { get; set; }
	public long? LastUpdatedAt { get; set; }

	public Deal(Guid id,
		decimal? revenue,
		string dealStatus,
		DateTime endDate,
		Guid clientId) : base(id)
	{
		AccountManagerDeals = new Collection<AccountManagerDeal>();
		DealContacts = new Collection<DealContact>();
		Revenue = revenue;
		DealStatus = dealStatus;
		EndDate = endDate;
		ClientId = clientId;
	}
}
