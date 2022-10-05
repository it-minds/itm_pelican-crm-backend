using Pelican.Domain.Primitives;

namespace Pelican.Domain.Entities;
public class Location : Entity
{
	public string CityName { get; set; }
	public Supplier? Supplier { get; set; }
	public Guid SupplierId { get; set; }
	public Location(Guid id, string cityName, Guid supplierId) : base(id)
	{
		CityName = cityName;
		SupplierId = supplierId;
	}
}
