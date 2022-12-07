using HotChocolate;
using Pelican.Domain.Primitives;

namespace Pelican.Domain.Entities;
public class Deal : Entity, ITimeTracked
{
	private string? _name;
	private string? _status;
	private string? _description;

	public Deal(Guid id) : base(id) { }

	public Deal() { }

	public string Source { get; set; } = string.Empty;

	public string SourceId { get; set; } = string.Empty;

	public string? SourceOwnerId { get; set; }

	public long? EndDate { get; set; }

	public long? StartDate { get; set; }

	public long? LastContactDate { get; set; }

	public Guid? ClientId { get; set; }

	public Client? Client { get; set; }

	public ICollection<AccountManagerDeal> AccountManagerDeals { get; set; } = new List<AccountManagerDeal>();

	public ICollection<DealContact> DealContacts { get; set; } = new List<DealContact>();

	public long CreatedAt { get; set; }

	public long? LastUpdatedAt { get; set; }

	public string? Status
	{
		get => _status;
		set => _status = value!.Length > StringLengths.DealStatus
			? value[..(StringLengths.DealStatus - 3)] + "..."
			: value;
	}

	public string? Name
	{
		get => _name;
		set => _name = value?.Length > StringLengths.DealName
			? value[..(StringLengths.DealName - 3)] + "..."
			: value;
	}

	public string? Description
	{
		get => _description;
		set => _description = value?.Length > StringLengths.DealDescription
			? value[..(StringLengths.DealDescription - 3)] + "..."
			: value;
	}

	[GraphQLIgnore]
	public virtual Deal UpdateProperty(string propertyName, string propertyValue)
	{
		switch (propertyName)
		{
			case "enddate":
				EndDate = long.Parse(propertyValue);
				break;
			case "startdate":
				StartDate = long.Parse(propertyValue);
				break;
			case "dealstage":
				Status = propertyValue;
				break;
			case "notes_last_contacted":
				LastContactDate = long.Parse(propertyValue);
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
	public virtual void UpdatePropertiesFromDeal(Deal deal)
	{
		EndDate = deal.EndDate;
		StartDate = deal.StartDate;
		DealStatus = deal.DealStatus;
		LastContactDate = deal.LastContactDate;
		Name = deal.Name;
		Description = deal.Description;
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

		if (oldRelation.SourceAccountManagerId != accountManager.SourceId)
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
				.FirstOrDefault(contact => contact.SourceId == dealContact.SourceContactId
					&& contact.Source == dealContact.Contact.Source);

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
