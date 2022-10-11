using Pelican.Domain.Primitives;

namespace Pelican.Domain.Entities;
public class Deal : Entity, ITimeTracked
{
	public string HubSpotId { get; set; }

	public decimal? Revenue { get; set; }

	public string CurrencyCode { get; set; }

	public string DealStatus { get; set; }

	public DateTime EndDate { get; set; }

	public Guid ClientId { get; set; }

	public Client? Client { get; set; }

	public ICollection<AccountManagerDeal> AccountManagerDeals { get; set; }

	public ICollection<DealContact> DealContacts { get; set; }

	public long CreatedAt { get; set; }

	public long? LastUpdatedAt { get; set; }

	public Deal(Guid id) : base(id)
	{
	}
	public Deal() { }

}
