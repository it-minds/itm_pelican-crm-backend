using HotChocolate;
using Pelican.Domain.Primitives;

namespace Pelican.Domain.Entities;
public class Deal : Entity, ITimeTracked
{
	public string HubSpotId { get; set; }

	public string? HubSpotOwnerId { get; set; }

	private string? _dealStatus;
	public string? DealStatus
	{
		get => _dealStatus;
		set
		{
			_dealStatus = value!.Length > StringLengths.DealStatus ? value.Substring(0, StringLengths.DealStatus - 3) + ("...") : value;
		}
	}

	public long? EndDate { get; set; }

	public long? StartDate { get; set; }

	public long? LastContactDate { get; set; }

	private string? _name;
	public string? Name
	{
		get => _name;
		set
		{
			_name = value!.Length > StringLengths.DealName ? value.Substring(0, StringLengths.DealName - 3) + ("...") : value;
		}
	}

	private string? _description;
	public string? Description
	{
		get => _description;
		set
		{
			_description = value!.Length > StringLengths.DealDescription ? value.Substring(0, StringLengths.DealDescription - 3) + ("...") : value;
		}
	}

	public Guid? ClientId { get; set; }

	public Client? Client { get; set; }


	public ICollection<AccountManagerDeal> AccountManagerDeals { get; set; } = new List<AccountManagerDeal>();

	public ICollection<DealContact> DealContacts { get; set; } = new List<DealContact>();


	public long CreatedAt { get; set; }

	public long? LastUpdatedAt { get; set; }


	public Deal(Guid id) : base(id) { }

	public Deal() { }

	[GraphQLIgnore]
	public virtual Deal UpdateProperty(string propertyName, string propertyValue)
	{
		switch (propertyName)
		{
			case "enddate":
				EndDate = Convert.ToDateTime(propertyValue).Ticks;
				break;
			case "startdate":
				StartDate = Convert.ToDateTime(propertyValue).Ticks;
				break;
			case "dealstage":
				DealStatus = propertyValue;
				break;
			case "notes_last_contacted":
				LastContactDate = Convert.ToDateTime(propertyValue).Ticks;
				break;
			case "dealname":
				Name = propertyValue;
				break;
			case "description":
				Description = propertyValue;
				break;
			default:
				throw new InvalidOperationException("Invalid field");
		}
		return this;
	}

	[GraphQLIgnore]
	public virtual Deal FillOutAssociations(AccountManager? accountManager, Client? client, List<Contact>? contacts)
	{
		FillOutAccountManager(accountManager);
		Client = client;
		FillOutDealContacts(contacts);

		return this;
	}

	[GraphQLIgnore]
	public virtual void FillOutAccountManager(AccountManager? accountManager)
	{
		if (accountManager is null)
		{
			return;
		}

		AccountManagerDeal? oldRelation = AccountManagerDeals
			.FirstOrDefault(a => a.IsActive == true);

		if (oldRelation is null)
		{
			AccountManagerDeals.Add(AccountManagerDeal.Create(this, accountManager));
			return;
		}

		if (oldRelation.HubSpotAccountManagerId != accountManager.HubSpotId)
		{
			oldRelation.Deactivate();

			AccountManagerDeals.Add(AccountManagerDeal.Create(this, accountManager));
		}
		else
		{
			oldRelation.AccountManager = accountManager;
			oldRelation.AccountManagerId = accountManager.Id;
		}
	}

	private void FillOutDealContacts(List<Contact>? contacts)
	{
		if (contacts is null)
		{
			DealContacts.Clear();
			return;
		}

		foreach (DealContact dealContact in DealContacts)
		{
			Contact? matchingContact = contacts
				.FirstOrDefault(contact => contact.HubSpotId == dealContact.HubSpotContactId);

			if (matchingContact is null)
			{
				continue;
			}

			dealContact.Contact = matchingContact;
			dealContact.ContactId = matchingContact.Id;
		}

		DealContacts = DealContacts
			.Where(dc => dc.Contact is not null)
			.ToList();
	}
}
