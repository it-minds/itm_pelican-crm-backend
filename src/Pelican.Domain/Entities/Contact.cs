using HotChocolate;
using Pelican.Domain.Primitives;

namespace Pelican.Domain.Entities;
public class Contact : Entity, ITimeTracked
{
	private string? _firstName;
	private string? _lastName;
	private string? _phoneNumber;
	private string? _jobTitle;
	private string? _email;

	public Contact(Guid id) : base(id) { }
	public Contact() { }

	public string Source { get; set; } = string.Empty;

	public string SourceId { get; set; } = string.Empty;

	public string? SourceOwnerId { get; set; }


	public string? FirstName
	{
		get => _firstName;
		set
		{
			_firstName = value?.Length > StringLengths.Name
				? value.Substring(0, StringLengths.Name - 3) + ("...")
				: value;
		}
	}
	public string? LastName
	{
		get => _lastName;
		set
		{
			_lastName = value?.Length > StringLengths.Name
				? value.Substring(0, StringLengths.Name - 3) + ("...")
				: value;
		}
	}

	public string? PhoneNumber
	{
		get => _phoneNumber;
		set
		{
			_phoneNumber = value?.Length > StringLengths.PhoneNumber
				? value.Substring(0, StringLengths.PhoneNumber - 3) + ("...")
				: value;
		}
	}

	public string? Email
	{
		get => _email;
		set
		{
			_email = value?.Length > StringLengths.Email
				? value.Substring(0, StringLengths.Email - 3) + ("...")
				: value;
		}
	}

	public string? JobTitle
	{
		get => _jobTitle;
		set
		{
			_jobTitle = value?.Length > StringLengths.JobTitle
				? value.Substring(0, StringLengths.JobTitle - 3) + ("...")
				: value;
		}
	}

	public ICollection<ClientContact> ClientContacts { get; set; } = new List<ClientContact>();

	public ICollection<DealContact> DealContacts { get; set; } = new List<DealContact>();

	public long CreatedAt { get; set; }

	public long? LastUpdatedAt { get; set; }

	[GraphQLIgnore]
	public virtual Contact UpdateProperty(string propertyName, string propertyValue)
	{
		switch (propertyName)
		{
			case "firstname":
				FirstName = propertyValue;
				break;
			case "lastname":
				LastName = propertyValue;
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
			case "hs_all_owner_ids":
				SourceOwnerId = propertyValue;
				break;
			default:
				throw new InvalidOperationException("Invalid field");
		}
		return this;
	}

	[GraphQLIgnore]
	public virtual void UpdateDealContacts(ICollection<DealContact>? currentHubSpotDealContacts)
	{
		if (currentHubSpotDealContacts is null)
		{
			return;
		}

		foreach (DealContact dealContact in DealContacts.Where(dc => dc.IsActive))
		{
			if (!currentHubSpotDealContacts.Any(currentHubSpotDealContact => currentHubSpotDealContact.SourceDealId == dealContact.SourceDealId
			&& currentHubSpotDealContact.Source == Sources.HubSpot))
			{
				dealContact.Deactivate();
			}
		}

		foreach (DealContact dealContact in currentHubSpotDealContacts)
		{
			if (!DealContacts.Any(dc => dc.SourceDealId == dealContact.SourceDealId
			&& dc.IsActive
			&& dc.Source == Sources.HubSpot))
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
				.FirstOrDefault(client => client.SourceId == cc.SourceClientId && client.Source == Sources.HubSpot);

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
				.FirstOrDefault(deal => deal.SourceId == dc.SourceDealId && dc.Source == Sources.HubSpot);

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
