using Pelican.Domain.Primitives;

namespace Pelican.Domain.Entities;
public class Contact : Entity, ITimeTracked
{
	public string Name { get; set; }
	public string? PhoneNumber { get; set; }
	public string Email { get; set; }
	public string? JobTitle { get; set; }
	public string? LinkedInUrl { get; set; }
	public ICollection<ClientContact> ClientContacts { get; set; }
	public ICollection<DealContact> DealContacts { get; set; }
	public long CreatedAt { get; set; }
	public long? LastUpdatedAt { get; set; }

	public Contact(Guid id) : base(id)
	{
	}
	public Contact() { }

}
