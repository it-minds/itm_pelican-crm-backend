using Pelican.Domain.Primitives;

namespace Pelican.Domain.Entities;
public class Deal : Entity, ITimeTracked
{
	public string HubSpotId { get; set; }

	public string HubSpotOwnerId { get; set; }


	public decimal? Revenue { get; set; }

	public string? CurrencyCode { get; set; }

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


	public Deal UpdateProperty(string propertyName, string propertyValue)
	{
		switch (propertyName)
		{
			case "closedate":
				{
					bool hasLongValue = long.TryParse(propertyValue, out long valueLong);
					if (!hasLongValue)
					{
						throw new InvalidOperationException();
					}
					EndDate = new DateTime(valueLong, DateTimeKind.Utc);
					break;
				}
			case "amount":
				{
					bool hasDecimalValue = decimal.TryParse(propertyValue, out decimal valueDecimal);
					if (!hasDecimalValue)
					{
						throw new InvalidOperationException();
					}
					Revenue = valueDecimal;
					break;
				}
			case "dealstage":
				DealStatus = propertyValue;
				break;
			case "deal_currency_code":
				CurrencyCode = propertyValue;
				break;
			default:
				break;
		}

		return this;
	}

	public Deal AttachAccountmManager(AccountManager? accountManager)
	{
		if (accountManager is not null)
		{
			AccountManagerDeals.Add(AccountManagerDeal.Create(this, accountManager));
		}

		return this;
	}

	public Deal AttachClient(Client? client)
	{
		Client = client;

		return this;
	}

	public Deal AttandContacts(List<Contact> contacts)
	{
		foreach (Contact contact in contacts)
		{
			DealContacts.Add(DealContact.Create(this, contact));
		}

		return this;
	}
}
