using Pelican.Domain.Primitives;

namespace Pelican.Domain.Entities;
public class ContactPerson : Entity, ITimeTracked
{
	public string Name { get; set; }
	public string PhoneNumber { get; set; }
	public string Email { get; set; }
	public string? LinkedIn { get; set; }
	public ICollection<ClientContactPerson> ClientContactPersons { get; set; }
	public ICollection<DealContactPerson> DealContactPersons { get; set; }
	public long CreatedAt { get; set; }
	public long? LastUpdatedAt { get; set; }

	public ContactPerson(Guid id, string name,
		string phoneNumber,
		string email,
		string? linkedIn,
		ICollection<ClientContactPerson> clientContactPersons,
		ICollection<DealContactPerson> dealContactPersons) : base(id)
	{
		Name = name;
		PhoneNumber = phoneNumber;
		Email = email;
		LinkedIn = linkedIn;
		ClientContactPersons = clientContactPersons;
		DealContactPersons = dealContactPersons;
	}
}
