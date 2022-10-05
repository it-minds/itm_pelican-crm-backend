using Pelican.Domain.Primitives;

namespace Pelican.Domain.Entities;

public class Supplier : Entity, ITimeTracked
{
	public string Name { get; set; }
	public string PictureUrl { get; set; }
	public string PhoneNumber { get; set; }
	public string Email { get; set; }
	public string LinkedIn { get; set; }
	public string WebsiteUrl { get; set; }
	public ICollection<string> OfficeLocations { get; set; }
	public ICollection<AccountManager> AccountManagers { get; set; }
	public long CreatedAt { get; set; }
	public long? LastUpdatedAt { get; set; }

	public Supplier(Guid id, string name,
		string pictureUrl,
		string phoneNumber,
		string email,
		string linkedIn,
		string websiteUrl,
		ICollection<string> officeLocations,
		ICollection<AccountManager> accountManagers) : base(id)
	{
		Name = name;
		PictureUrl = pictureUrl;
		PhoneNumber = phoneNumber;
		Email = email;
		LinkedIn = linkedIn;
		WebsiteUrl = websiteUrl;
		OfficeLocations = officeLocations;
		AccountManagers = accountManagers;
	}
}
