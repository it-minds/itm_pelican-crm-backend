using HotChocolate;
using Pelican.Domain.Primitives;

namespace Pelican.Domain.Entities;
public class ClientContact : Entity, ITimeTracked
{
	public bool IsActive { get; set; }


	public Guid ClientId { get; set; }

	public string HubSpotClientId { get; set; } = string.Empty;

	public Client Client { get; set; }


	public string HubSpotContactId { get; set; } = string.Empty;

	public Guid ContactId { get; set; }

	public Contact Contact { get; set; }


	public long CreatedAt { get; set; }

	public long? LastUpdatedAt { get; set; }


	public ClientContact(Guid id) : base(id) { }

	public ClientContact() { }

	[GraphQLIgnore]
	public static ClientContact Create(Client client, Contact contact)
	{
		return new(Guid.NewGuid())
		{
			Client = client,
			ClientId = client.Id,
			HubSpotClientId = client.HubSpotId,
			Contact = contact,
			ContactId = contact.Id,
			HubSpotContactId = contact.HubSpotId,
			IsActive = true,
		};
	}

	[GraphQLIgnore]
	public virtual void Deactivate() => IsActive = false;
}
