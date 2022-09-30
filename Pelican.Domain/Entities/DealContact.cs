using Pelican.Domain.Primitives;

namespace Pelican.Domain.Entities;
public class DealContact : Entity, ITimeTracked
{
	public Guid DealId { get; set; }
	public Guid ContactId { get; set; }
	public bool IsActive { get; set; }
	public Deal? Deal { get; set; }
	public Contact? Contact { get; set; }
	public long CreatedAt { get; set; }
	public long? LastUpdatedAt { get; set; }

	public DealContact(Guid id, Guid dealId,
		Guid contactId,
		bool isActive) : base(id)
	{
		DealId = dealId;
		ContactId = contactId;
		IsActive = isActive;
	}
}
