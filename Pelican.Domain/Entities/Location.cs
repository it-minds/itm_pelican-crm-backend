using Pelican.Domain.Primitives;

namespace Pelican.Domain.Entities;
public class Location : Entity<Guid>, ITimeTracked
{
	public string CityName { get; set; }
	public Supplier? Supplier { get; set; }
	public Guid SupplierId { get; set; }
	public long CreatedAt { get; set; }
	public long? LastUpdatedAt { get; set; }

	public Location()
	{
		Id = Guid.NewGuid();
	}
}
