using Pelican.Domain.Primitives;

namespace Pelican.Domain.Entities;
public class DealContactPerson : Entity, ITimeTracked
{
	public Guid DealId { get; set; }
	public Guid ContactPersonId { get; set; }
	public bool IsActive { get; set; }
	public long CreatedAt { get; set; }
	public long? LastUpdatedAt { get; set; }

	public DealContactPerson(Guid id, Guid dealId,
		Guid contactPersonId,
		bool isActive) : base(id)
	{
		DealId = dealId;
		ContactPersonId = contactPersonId;
		IsActive = isActive;
	}
}
