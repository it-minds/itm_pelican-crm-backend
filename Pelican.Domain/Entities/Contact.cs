using Pelican.Domain.Primitives;

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
}
