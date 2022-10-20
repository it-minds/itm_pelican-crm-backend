using Pelican.Domain.Primitives;

namespace Pelican.Domain.Entities;

public class DealContact : Entity, ITimeTracked
{
	public bool IsActive { get; set; }


	public Guid DealId { get; set; }

	public string HubSpotDealId { get; set; }

	public Deal Deal { get; set; }


	public Guid ContactId { get; set; }

	public string HubSpotContactId { get; set; }

	public Contact Contact { get; set; }


	public long CreatedAt { get; set; }

	public long? LastUpdatedAt { get; set; }

	public DealContact(Guid id) : base(id) { }

	public DealContact() { }
}
