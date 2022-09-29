using Pelican.Domain.Primitives;

namespace Pelican.Domain.Entities;
public class ClientContact : Entity, ITimeTracked
{
	public Guid ClientId { get; set; }
	public Guid ContactId { get; set; }
	public Client Client { get; set; }
	public Contact Contact { get; set; }
	public bool IsActive { get; set; }
	public long CreatedAt { get; set; }
	public long? LastUpdatedAt { get; set; }

	public ClientContact(Guid id, Guid clientId,
		Guid contactId,
		bool isActive,
		Client client, Contact contact) : base(id)
	{
		ClientId = clientId;
		ContactId = contactId;
		IsActive = isActive;
		Client = client;
		Contact = contact;
	}
}
