using Pelican.Domain.Primitives;

namespace Pelican.Domain.Entities;
public class Deal : Entity<Guid>, ITimeTracked
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

	public Deal()
	{
		Id = Guid.NewGuid();
	}
}
