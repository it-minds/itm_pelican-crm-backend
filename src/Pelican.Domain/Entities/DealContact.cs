using HotChocolate;
using Pelican.Domain.Primitives;

namespace Pelican.Domain.Entities;

public class DealContact : Entity, ITimeTracked
{
	public DealContact(Guid id) : base(id) { }

	public DealContact() { }

	public bool IsActive { get; set; }

	public Guid DealId { get; set; }

	public string SourceDealId { get; set; } = string.Empty;

	public Deal Deal { get; set; }


	public Guid ContactId { get; set; }

	public string SourceContactId { get; set; } = string.Empty;

	public Contact Contact { get; set; }

	public string Source { get; set; }

	public long CreatedAt { get; set; }

	public long? LastUpdatedAt { get; set; }

	[GraphQLIgnore]
	public static DealContact Create(Deal deal, Contact contact)
	{
		return new DealContact(Guid.NewGuid())
		{
			Deal = deal,
			DealId = deal.Id,
			SourceDealId = deal.SourceId,
			Contact = contact,
			ContactId = contact.Id,
			SourceContactId = contact.SourceId,
			IsActive = true,
			Source = deal.Source,
		};
	}

	[GraphQLIgnore]
	public virtual void Deactivate() => IsActive = false;
}
