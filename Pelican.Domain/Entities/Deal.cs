using Pelican.Domain.Primitives;

namespace Pelican.Domain.Entities;
public class Deal : Entity, ITimeTracked
{
	public string HubSpotId { get; set; }

	public string HubSpotOwnerId { get; set; }


	public decimal? Revenue { get; set; }

	public string? CurrencyCode { get; set; }

	public string? DealStatus { get; set; }

	public DateTime? EndDate { get; set; }


	public Guid? ClientId { get; set; }

	public Client? Client { get; set; }


	public ICollection<AccountManagerDeal> AccountManagerDeals { get; set; } = new List<AccountManagerDeal>();

	public ICollection<DealContact>? DealContacts { get; set; } = new List<DealContact>();


	public long CreatedAt { get; set; }

	public long? LastUpdatedAt { get; set; }


	public Deal(Guid id) : base(id) { }
	public Deal() { }

}
