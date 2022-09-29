using Pelican.Domain.Primitives;

namespace Pelican.Domain.Entities;
public class Client : Entity, ITimeTracked
{
	public string Name { get; set; }
	public string PictureUrl { get; set; }
	public string OfficeLocation { get; set; }
	public string Segment { get; set; }
	public string Classification { get; set; }
	public ICollection<Deal> Deals { get; set; }
	public ICollection<ClientContact> ClientContacts { get; set; }
	public long CreatedAt { get; set; }
	public long? LastUpdatedAt { get; set; }

	public Client(Guid id, string name,
		string pictureUrl,
		string officeLocation,
		string segment,
		string classification,
		ICollection<Deal> deals,
		ICollection<ClientContact> clientContacts) : base(id)
	{
		Name = name;
		PictureUrl = pictureUrl;
		OfficeLocation = officeLocation;
		Segment = segment;
		Classification = classification;
		Deals = deals;
		ClientContacts = clientContacts;
	}
}
