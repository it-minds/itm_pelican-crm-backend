using Pelican.Domain.Primitives;

namespace Pelican.Domain.Entities;
public class Location : Entity, ITimeTracked
{
	private string _cityName;
	public string CityName
	{
		get => _cityName;
		set
		{
			_cityName = value.Length > StringLengths.OfficeLocation ? value.Substring(0, StringLengths.OfficeLocation - 3) + ("...") : value;
		}
	}

	public Supplier Supplier { get; set; }

	public Guid SupplierId { get; set; }


	public long CreatedAt { get; set; }

	public long? LastUpdatedAt { get; set; }

	public Location(Guid id) : base(id) { }

	public Location() { }
}
