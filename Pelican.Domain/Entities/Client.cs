using Pelican.Domain.Primitives;

namespace Pelican.Domain.Entities;
public class Client : Entity, ITimeTracked
{
	public string Name { get; set; }

	public string HubSpotId { get; set; }

	public string? PictureUrl { get; set; }

	public string? OfficeLocation { get; set; }

	public ICollection<Deal> Deals { get; set; } = new List<Deal>();

	public ICollection<ClientContact> ClientContacts { get; set; } = new List<ClientContact>();

	public long CreatedAt { get; set; }

	public long? LastUpdatedAt { get; set; }

	public string? Website { get; set; }

	public Client(Guid id) : base(id) { }

	public Client() { }
}
