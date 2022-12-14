using HotChocolate;
using Pelican.Domain.Extensions;
using Pelican.Domain.Primitives;
using Pelican.Domain.Shared;

namespace Pelican.Domain.Entities;
public class Deal : Entity, ITimeTracked
{
	private string? _name;
	private string? _status;
	private string? _description;

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
		set => _status = value?.CheckAndShortenExceedingString(StringLengths.DealStatus);
	}

	public string? Name
	{
		get => _name;
		set => _name = value?.CheckAndShortenExceedingString(StringLengths.DealName);
	}

	public string? Description
	{
		get => _description;
		set => _description = value?.CheckAndShortenExceedingString(StringLengths.DealDescription);
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
			case "hs_all_owner_ids":
				SourceOwnerId = propertyValue;
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
		Status = deal.Status;
		LastContactDate = deal.LastContactDate;
		Name = deal.Name;
		Description = deal.Description;
		SourceOwnerId = deal.SourceOwnerId;
	}

	[GraphQLIgnore]
	public virtual void UpdateAccountManager(AccountManager? accountManager)
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

	[GraphQLIgnore]
	public virtual void SetAccountManager(AccountManager? accountManager)
	{
		if (accountManager is not null)
		{
			AccountManagerDeal accountManagerDeal = AccountManagerDeal.Create(this, accountManager);
			AccountManagerDeals = new List<AccountManagerDeal>() { accountManagerDeal };
			accountManager.AccountManagerDeals.Add(accountManagerDeal);
		}
	}

	[GraphQLIgnore]
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

	[GraphQLIgnore]
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
