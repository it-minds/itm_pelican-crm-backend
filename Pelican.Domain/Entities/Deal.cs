using HotChocolate;
using Pelican.Domain.Primitives;

namespace Pelican.Domain.Entities;
public class Deal : Entity, ITimeTracked
{
	public string HubSpotId { get; set; }

	public string? HubSpotOwnerId { get; set; }

	public string? DealStatus { get; set; }

	public DateTime? EndDate { get; set; }


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
			case "closedate":
				{
					bool hasLongValue = long.TryParse(propertyValue, out long value);
					if (!hasLongValue)
					{
						throw new InvalidOperationException("Invalid date format");
					}
					DateTime date = DateTimeOffset.FromUnixTimeMilliseconds(value).Date;
					EndDate = date;
					break;
				}
			case "dealstage":
				DealStatus = propertyValue;
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
