using HotChocolate;
using Pelican.Domain.Primitives;

namespace Pelican.Domain.Entities;

public class DealContact : Entity, ITimeTracked
{
	public bool IsActive { get; set; }


	public Guid DealId { get; set; }

	public string HubSpotDealId { get; set; } = string.Empty;

	public Deal Deal { get; set; }


	public Guid ContactId { get; set; }

	public string HubSpotContactId { get; set; } = string.Empty;

	public Contact Contact { get; set; }


	public long CreatedAt { get; set; }

	public long? LastUpdatedAt { get; set; }


	public DealContact(Guid id) : base(id) { }

	public DealContact() { }

	[GraphQLIgnore]
	public static DealContact Create(Deal deal, Contact contact)
	{
		return new DealContact(Guid.NewGuid())
		{
			Deal = deal,
			DealId = deal.Id,
			HubSpotDealId = deal.HubSpotId,
			Contact = contact,
			ContactId = contact.Id,
			HubSpotContactId = contact.HubSpotId,
			IsActive = true,
		};
	}

	[GraphQLIgnore]
	public virtual void Deactivate() => IsActive = false;
}
