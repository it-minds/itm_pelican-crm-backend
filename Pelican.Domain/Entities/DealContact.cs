using Pelican.Domain.Primitives;

namespace Pelican.Domain.Entities;
public class DealContact : Entity<Guid>, ITimeTracked
{
	public Guid DealId { get; set; }
	public Guid ContactId { get; set; }
	public bool IsActive { get; set; }
	public Deal? Deal { get; set; }
	public Contact? Contact { get; set; }
	public long CreatedAt { get; set; }
	public long? LastUpdatedAt { get; set; }

	public DealContact()
	{
		Id = Guid.NewGuid();
	}
}
