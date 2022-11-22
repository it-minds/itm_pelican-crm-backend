using HotChocolate;
using Pelican.Domain.Primitives;

namespace Pelican.Domain.Entities;
public class Contact : Entity, ITimeTracked
{
	public string HubSpotId { get; set; }

	public string? HubSpotOwnerId { get; set; }


	public string? FirstName { get; set; }

	public string? LastName { get; set; }


	public string? PhoneNumber { get; set; }

	public string? Email { get; set; }

	public string? JobTitle { get; set; }


	public ICollection<ClientContact> ClientContacts { get; set; } = new List<ClientContact>();

	public ICollection<DealContact> DealContacts { get; set; } = new List<DealContact>();


	public long CreatedAt { get; set; }

	public long? LastUpdatedAt { get; set; }


	public Contact(Guid id) : base(id) { }
	public Contact() { }

	[GraphQLIgnore]
	public virtual Contact UpdateProperty(string propertyName, string propertyValue)
	{
		switch (propertyName)
		{
			case "firstname":
				FirstName = propertyValue.Length > StringLengths.Name ? propertyValue.Substring(0, StringLengths.Name - 3) + ("...") : propertyValue;
				break;
			case "lastname":
				LastName = propertyValue.Length > StringLengths.Name ? propertyValue.Substring(0, StringLengths.Name - 3) + ("...") : propertyValue;
				break;
			case "email":
				Email = propertyValue.Length > StringLengths.Email ? propertyValue.Substring(0, StringLengths.Email - 3) + ("...") : propertyValue;
				break;
			case "phone":
			case "mobilephone":
				PhoneNumber = propertyValue.Length > StringLengths.PhoneNumber ? propertyValue.Substring(0, StringLengths.PhoneNumber - 3) + ("...") : propertyValue;
				break;
			case "jobtitle":
				JobTitle = propertyValue.Length > StringLengths.JobTitle ? propertyValue.Substring(0, StringLengths.JobTitle - 3) + ("...") : propertyValue;
				break;
			case "hs_all_owner_ids":
				HubSpotOwnerId = propertyValue;
				break;
			default:
				throw new InvalidOperationException("Invalid field");
		}
		return this;
	}

	[GraphQLIgnore]
	public virtual void UpdateDealContacts(ICollection<DealContact>? currectHubSpotDealContacts)
	{
		if (currectHubSpotDealContacts is null)
		{
			return;
		}

		foreach (DealContact dealContact in DealContacts.Where(dc => dc.IsActive))
		{
			if (!currectHubSpotDealContacts.Any(currectHubSpotDealContact => currectHubSpotDealContact.HubSpotDealId == dealContact.HubSpotDealId))
			{
				dealContact.Deactivate();
			}
		}

		foreach (DealContact dealContact in currectHubSpotDealContacts)
		{
			if (!DealContacts.Any(dc => dc.HubSpotDealId == dealContact.HubSpotDealId && dc.IsActive))
			{
				DealContacts.Add(dealContact);
			}
		}
	}

	[GraphQLIgnore]
	public virtual Contact FillOutAssociations(IEnumerable<Client>? clients, IEnumerable<Deal>? deals)
	{
		FillOutClient(clients);
		FillOutDealContacts(deals);

		return this;
	}

	private void FillOutClient(IEnumerable<Client>? clients)
	{
		if (clients is null)
		{
			ClientContacts.Clear();
			return;
		}

		ClientContacts = ClientContacts
			.Select(cc =>
			{
				Client? matchingClient = clients
				.FirstOrDefault(client => client.HubSpotId == cc.HubSpotClientId);

				if (matchingClient is not null)
				{
					cc.Client = matchingClient;
					cc.ClientId = matchingClient.Id;
				}

				return cc;
			})
			.Where(cc => cc.Client is not null)
			.ToList();
	}

	private void FillOutDealContacts(IEnumerable<Deal>? deals)
	{
		if (deals is null)
		{
			DealContacts.Clear();
			return;
		}

		DealContacts = DealContacts
			.Select(dc =>
			{
				Deal? matchingDeal = deals
				.FirstOrDefault(deal => deal.HubSpotId == dc.HubSpotDealId);

				if (matchingDeal is not null)
				{
					dc.Deal = matchingDeal;
					dc.DealId = matchingDeal.Id;
				}

				return dc;
			})
			.Where(dc => dc.Deal is not null)
			.ToList();
	}
}
