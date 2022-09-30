using Pelican.Domain.Primitives;

namespace Pelican.Domain.Entities;

public class Supplier : Entity, ITimeTracked
{
	public string Name { get; set; }
	public string? PictureUrl { get; set; }
	public string? PhoneNumber { get; set; }
	public string? Email { get; set; }
	public string? LinkedInUrl { get; set; }
	public string? WebsiteUrl { get; set; }
	public ICollection<Location>? OfficeLocations { get; set; }
	public ICollection<AccountManager>? AccountManagers { get; set; }
	public long CreatedAt { get; set; }
	public long? LastUpdatedAt { get; set; }

	public Supplier(Guid id, string name,
		string? pictureUrl,
		string? phoneNumber,
		string? email,
		string? linkedInUrl,
		string? websiteUrl) : base(id)
	{
		Name = name;
		PictureUrl = pictureUrl;
		PhoneNumber = phoneNumber;
		Email = email;
		LinkedInUrl = linkedInUrl;
		WebsiteUrl = websiteUrl;
	}
}
