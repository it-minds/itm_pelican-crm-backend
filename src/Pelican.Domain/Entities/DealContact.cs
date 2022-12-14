using HotChocolate;
using Pelican.Domain.Primitives;

namespace Pelican.Domain.Entities;

public class DealContact : Entity, ITimeTracked
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
	public DealContact() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

	public bool IsActive { get; set; }

	public Guid DealId { get; set; }

	public string SourceDealId { get; set; } = string.Empty;

	public Deal Deal { get; set; }


	public Guid ContactId { get; set; }

	public string SourceContactId { get; set; } = string.Empty;

	public Contact Contact { get; set; }

	public long CreatedAt { get; set; }

	public long? LastUpdatedAt { get; set; }

	[GraphQLIgnore]
	public static DealContact Create(Deal deal, Contact contact)
	{
		return new DealContact()
		{
			Deal = deal,
			DealId = deal.Id,
			SourceDealId = deal.SourceId,
			Contact = contact,
			ContactId = contact.Id,
			SourceContactId = contact.SourceId,
			IsActive = true,
		};
	}

	[GraphQLIgnore]
	public virtual void Deactivate() => IsActive = false;
}
