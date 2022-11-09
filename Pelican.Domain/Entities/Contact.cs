﻿using Pelican.Domain.Primitives;

namespace Pelican.Domain.Entities;
public class Contact : Entity, ITimeTracked
{
	public string HubSpotId { get; set; }

	public string? HubSpotOwnerId { get; set; }


	public string? Firstname { get; set; }

	public string? Lastname { get; set; }


	public string? PhoneNumber { get; set; }

	public string? Email { get; set; }

	public string? JobTitle { get; set; }


	public ICollection<ClientContact> ClientContacts { get; set; } = new List<ClientContact>();

	public ICollection<DealContact> DealContacts { get; set; } = new List<DealContact>();


	public long CreatedAt { get; set; }

	public long? LastUpdatedAt { get; set; }


	public Contact(Guid id) : base(id) { }
	public Contact() { }


	public virtual Contact UpdateProperty(string propertyName, string propertyValue)
	{
		switch (propertyName)
		{
			case "firstname":
				Firstname = propertyValue;
				break;
			case "lastname":
				Lastname = propertyValue;
				break;
			case "email":
				Email = propertyValue;
				break;
			case "phone":
			case "mobilephone":
				PhoneNumber = propertyValue;
				break;
			case "jobtitle":
				JobTitle = propertyValue;
				break;
			default:
				throw new InvalidOperationException("Invalid field");
		}

		return this;
	}

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
