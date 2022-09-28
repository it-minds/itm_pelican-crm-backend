using Pelican.Domain.Primitives;

namespace Pelican.Domain.Entities;
public class ClientContactPerson : Entity, ITimeTracked
{
	public Guid ClientId { get; set; }
	public Guid ContactPersonId { get; set; }
	public bool IsActive { get; set; }
	public long CreatedAt { get; set; }
	public long? LastUpdatedAt { get; set; }

	public ClientContactPerson(Guid id, Guid clientId,
		Guid contactPersonId,
		bool isActive) : base(id)
	{
		ClientId = clientId;
		ContactPersonId = contactPersonId;
		IsActive = isActive;
	}
}
