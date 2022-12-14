using HotChocolate;
using Pelican.Domain.Primitives;

namespace Pelican.Domain.Entities;
public class Client : Entity, ITimeTracked
{
	private string _name = string.Empty;
	private string? _website { get; set; }

	public string? PictureUrl { get; set; }

	public Client(Guid id) : base(id) { }

	public Client() { }

	public string Source { get; set; } = string.Empty;

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

	public string SourceId { get; set; } = string.Empty;


	private string? _officeLocation;
	public string? OfficeLocation
	{
		get => _officeLocation;
		set
		{
			_officeLocation = value?.Length > StringLengths.OfficeLocation
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
			_website = value?.Length > StringLengths.Url
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
			if (item.Contact is not null)
			{
				continue;
			}

			Contact? matchingContact = contacts
				.FirstOrDefault(contacts => contacts.SourceId == item.SourceContactId && contacts.Source == item.Client.Source);

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
	public virtual void UpdateClientContacts(ICollection<ClientContact>? currentClientContacts)
	{
		if (currentClientContacts is null)
		{
			return;
		}

		foreach (ClientContact clientContact in ClientContacts.Where(dc => dc.IsActive))
		{
			if (!currentClientContacts.Any(currentClientContact => currentClientContact.SourceContactId == clientContact.SourceContactId
			&& currentClientContact.Client.Source == clientContact.Client.Source))
			{
				clientContact.Deactivate();
			}
		}

		foreach (ClientContact clientContact in currentClientContacts)
		{
			if (!ClientContacts.Any(dc => dc.SourceContactId == clientContact.SourceContactId
			&& dc.IsActive
			&& dc.Client.Source == clientContact.Client.Source))
			{
				ClientContacts.Add(clientContact);
			}
		}
	}

	[GraphQLIgnore]
	public virtual void UpdatePropertiesFromClient(Client client)
	{
		Name = client.Name;
		OfficeLocation = client.OfficeLocation;
		Website = client.Website;
	}
}
