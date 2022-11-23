using HotChocolate;
using Pelican.Domain.Primitives;

namespace Pelican.Domain.Entities;
public class Client : Entity, ITimeTracked
{
	public string Name { get; set; }

	public string HubSpotId { get; set; }

	public string? PictureUrl { get; set; }

	public string? OfficeLocation { get; set; }

	public ICollection<Deal> Deals { get; set; } = new List<Deal>();

	public ICollection<ClientContact> ClientContacts { get; set; } = new List<ClientContact>();

	public long CreatedAt { get; set; }

	public long? LastUpdatedAt { get; set; }

	public string? Website { get; set; }

	public Client(Guid id) : base(id) { }

	public Client() { }

	[GraphQLIgnore]
	public virtual Client UpdateProperty(string propertyName, string propertyValue)
	{
		switch (propertyName)
		{
			case "name":
				Name = propertyValue;
				break;
			case "city":
				OfficeLocation = propertyValue;
				break;
			case "website":
				Website = propertyValue;
				break;
			default:
				throw new InvalidOperationException($"{propertyName} is not a valid property on Client");
		}
		return this;
	}

	[GraphQLIgnore]
	public virtual void FillOutClientContacts(IEnumerable<Contact>? contacts)
	{
		if (contacts is null)
		{
			ClientContacts.Clear();
			return;
		}

		foreach (ClientContact item in ClientContacts)
		{
			Contact? matchingContact = contacts
				.FirstOrDefault(contacts => contacts.HubSpotId == item.HubSpotContactId);

			if (matchingContact is null)
			{
				continue;
			}

			item.Contact = matchingContact;
			item.ContactId = matchingContact.Id;
		}

		ClientContacts = ClientContacts
			.Where(cc => cc.Contact is not null)
			.ToList();
	}


	[GraphQLIgnore]
	public virtual void UpdateClientContacts(ICollection<ClientContact>? currectHubSpotClientContacts)
	{
		if (currectHubSpotClientContacts is null)
		{
			return;
		}

		foreach (ClientContact clientContact in ClientContacts.Where(dc => dc.IsActive))
		{
			if (!currectHubSpotClientContacts.Any(currectHubSpotClientContact => currectHubSpotClientContact.HubSpotClientId == clientContact.HubSpotClientId))
			{
				clientContact.Deactivate();
			}
		}

		foreach (ClientContact clientContact in currectHubSpotClientContacts)
		{
			if (!ClientContacts.Any(dc => dc.HubSpotClientId == clientContact.HubSpotClientId && dc.IsActive))
			{
				ClientContacts.Add(clientContact);
			}
		}
	}
}
