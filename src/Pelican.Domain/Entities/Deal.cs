using System.Collections.Generic;
using HotChocolate;
using Pelican.Domain.Primitives;
using Pelican.Domain.Shared;

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

	public AccountManagerDeal? ActiveAccountManagerDeal
	{
		get => AccountManagerDeals.FirstOrDefault(am => am.IsActive);
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
	public virtual void SetAccountManager(AccountManager? accountManager)
	{
		if (accountManager is null)
		{
			ActiveAccountManagerDeal?.Deactivate();
			return;
		}
		if (ActiveAccountManagerDeal?.SourceAccountManagerId != accountManager.SourceId)
		{
			ActiveAccountManagerDeal?.Deactivate();
			AccountManagerDeal accountManagerDeal = AccountManagerDeal.Create(this, accountManager);
			AccountManagerDeals.Add(accountManagerDeal);
			accountManager.AccountManagerDeals.Add(accountManagerDeal);
		}
	}

	public virtual void SetContacts(IEnumerable<Contact?>? contacts)
		=> DealContacts = contacts?
			.Select(contact =>
			{
				if (contact is not null)
				{
					DealContact dealContact = DealContact.Create(this, contact);
					contact.DealContacts.Add(dealContact);
					return dealContact;
				}
				return null;
			})
			.Where(dc => dc is not null)
			.ToList() as ICollection<DealContact> ?? new List<DealContact>();

	public virtual void SetClient(Client? client)
	{
		Client = client;
		ClientId = null;
		if (client is not null)
		{
			client.Deals.Add(this);
			ClientId = client.Id;
		}
	}
}
