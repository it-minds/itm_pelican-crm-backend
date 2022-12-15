using HotChocolate;
using Pelican.Domain.Primitives;

namespace Pelican.Domain.Entities;
public class Client : Entity, ITimeTracked
{
	private string _name = string.Empty;
	private string? _website { get; set; }

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
	public virtual void UpdateClientContacts(IEnumerable<ClientContact> clientContacts)
	{
		foreach (var item in ClientContacts.Where(x => x.IsActive))
		{
			if (!clientContacts.Any(cc => cc.SourceContactId == item.SourceContactId && cc.Client.Source == Source))
			{
				item.Deactivate();
			}
		}

		foreach (var item in clientContacts)
		{
			if (!ClientContacts.Any(cc => cc.SourceContactId == item.SourceContactId && cc.Client.Source == Source && cc.IsActive))
			{
				ClientContacts.Add(item);
			}
		}
	}

	[GraphQLIgnore]
	public virtual void UpdateDeals(IEnumerable<Deal> deals)
	{
		Deals = Deals
			.Where(deal => deals.Any(d => deal.SourceId == d.SourceId && deal.Source == d.Source))
			.ToList();

		foreach (var item in deals)
		{
			if (!Deals.Any(d => d.SourceId == item.SourceId && d.Source == item.Source))
			{
				Deals.Add(item);
			}
		}
	}

	[GraphQLIgnore]
	public virtual void SetClientContacts(IEnumerable<Contact?>? contacts)
	{
		ClientContacts = contacts?
			.Select(contact =>
			{
				if (contact is not null)
				{
					ClientContact clientContact = ClientContact.Create(this, contact);
					return clientContact;
				}
				return null;
			})
			.Where(cc => cc is not null)
			.ToList() as ICollection<ClientContact> ?? new List<ClientContact>();
	}

	[GraphQLIgnore]
	public virtual void SetDeals(IEnumerable<Deal?>? deals)
	{
		Deals = deals?
			.Where(deal => deal is not null)
			.ToList()! as ICollection<Deal> ?? new List<Deal>();

		foreach (Deal deal in Deals)
		{
			deal.ClientId = Id;
			deal.Client = this;
		}
	}

	[GraphQLIgnore]
	public virtual void UpdatePropertiesFromClient(Client client)
	{
		Name = client.Name;
		OfficeLocation = client.OfficeLocation;
		Website = client.Website;
		UpdateClientContacts(client.ClientContacts);
		UpdateDeals(client.Deals);
	}
}

