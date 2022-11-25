using HotChocolate;
using Pelican.Domain.Primitives;

namespace Pelican.Domain.Entities;
public class Client : Entity, ITimeTracked
{
	private string _name = string.Empty;
	public string? PictureUrl { get; set; }
	private string? _website { get; set; }

	public Client(Guid id) : base(id) { }

	public Client() { }

	public string Name
	{
		get => _name;
		set
		{
			_name = value.Length > StringLengths.Name
				? value.Substring(0, StringLengths.Name - 3) + ("...")
				: value;
		}
	}

	public string HubSpotId { get; set; } = string.Empty;


	private string? _officeLocation;
	public string? OfficeLocation
	{
		get => _officeLocation;
		set
		{
			_officeLocation = value!.Length > StringLengths.OfficeLocation
				? value.Substring(0, StringLengths.OfficeLocation - 3) + ("...")
				: value;
		}
	}

	public ICollection<Deal> Deals { get; set; } = new List<Deal>();

	public ICollection<ClientContact> ClientContacts { get; set; } = new List<ClientContact>();

	public long CreatedAt { get; set; }

	public long? LastUpdatedAt { get; set; }

	public string? Website
	{
		get => _website;
		set
		{
			_website = value!.Length > StringLengths.Url
				? value.Substring(0, StringLengths.Url - 3) + ("...")
				: value;
		}
	}

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
