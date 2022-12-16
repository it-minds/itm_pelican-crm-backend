using System.Collections.Generic;

namespace Pelican.Domain.Entities;
public class Contact : Entity, ITimeTracked
{
	private string? _firstName;
	private string? _lastName;
	private string? _phoneNumber;
	private string? _jobTitle;
	private string? _email;

	public Contact() { }

	public string Source { get; set; } = string.Empty;

	public string SourceId { get; set; } = string.Empty;

	public string? SourceOwnerId { get; set; }

	public ICollection<ClientContact> ClientContacts { get; set; } = new List<ClientContact>();

	public ICollection<DealContact> DealContacts { get; set; } = new List<DealContact>();

	public long CreatedAt { get; set; }

	public long? LastUpdatedAt { get; set; }


	public string? FirstName
	{
		get => _firstName;
		set => _firstName = value?.CheckAndShortenExceedingString(StringLengths.Name);
	}

	public string? LastName
	{
		get => _lastName;
		set => _lastName = value?.CheckAndShortenExceedingString(StringLengths.Name);
	}

	public string? PhoneNumber
	{
		get => _phoneNumber;
		set => _phoneNumber = value?.CheckAndShortenExceedingString(StringLengths.PhoneNumber);
	}

	public string? Email
	{
		get => _email;
		set => _email = value?.CheckAndShortenExceedingString(StringLengths.Email);
	}

	public string? JobTitle
	{
		get => _jobTitle;
		set => _jobTitle = value?.CheckAndShortenExceedingString(StringLengths.JobTitle);
	}

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
	public virtual void UpdatePropertiesFromContact(Contact contact)
	{
		FirstName = contact.FirstName;
		LastName = contact.LastName;
		Email = contact.Email;
		PhoneNumber = contact.PhoneNumber;
		JobTitle = contact.JobTitle;
		SourceOwnerId = contact.SourceOwnerId;
		UpdateDealContacts(contact.DealContacts);
	}

	[GraphQLIgnore]
	public virtual void UpdateDealContacts(ICollection<DealContact>? currentDealContacts)
	{
		if (currentDealContacts is null)
		{
			DealContacts
				.ToList()
				.ForEach(dc => dc.Deactivate());

			return;
		}

		foreach (DealContact dealContact in DealContacts.Where(dc => dc.IsActive))
		{
			if (!currentDealContacts.Any(currentDealContact => currentDealContact.SourceDealId == dealContact.SourceDealId
			&& currentDealContact.Deal.Source == Source))
			{
				dealContact.Deactivate();
			}
		}

		foreach (DealContact dealContact in currentDealContacts)
		{
			if (!DealContacts.Any(dc => dc.SourceDealId == dealContact.SourceDealId
			&& dc.IsActive
			&& Source == dealContact.Deal.Source))
			{
				DealContacts.Add(dealContact);
			}
		}
	}

	[GraphQLIgnore]
	public void SetDealContacts(IEnumerable<Deal?>? deals)
		=> DealContacts = deals?
			.Select(deal =>
			{
				if (deal is not null)
				{
					DealContact dealContact = DealContact.Create(deal, this);
					deal.DealContacts.Add(dealContact);
					return dealContact;
				}
				return null;
			})
			.Where(dc => dc is not null)
			.ToList() as ICollection<DealContact> ?? new List<DealContact>();

	[GraphQLIgnore]
	public void SetClientContacts(IEnumerable<Client?>? clients)
		=> ClientContacts = clients?
			.Select(client =>
			{
				if (client is not null)
				{
					ClientContact clientContact = ClientContact.Create(client, this);
					client.ClientContacts.Add(clientContact);
					return clientContact;
				}
				return null;
			})
			.Where(dc => dc is not null)
			.ToList() as ICollection<ClientContact> ?? new List<ClientContact>();
}
