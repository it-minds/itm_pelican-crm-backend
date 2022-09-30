using System.Collections.ObjectModel;
using Pelican.Domain.Primitives;

namespace Pelican.Domain.Entities;
public class AccountManager : Entity, ITimeTracked
{
	public string Name { get; set; }
	public string? PictureUrl { get; set; }
	public string? PhoneNumber { get; set; }
	public string Email { get; set; }
	public string? LinkedInUrl { get; set; }
	public Guid SupplierId { get; set; }
	public Supplier? Supplier { get; set; }
	public ICollection<AccountManagerDeal> AccountManagerDeals { get; set; }
	public long CreatedAt { get; set; }
	public long? LastUpdatedAt { get; set; }

	public AccountManager(Guid id, string name,
		string? pictureUrl,
		string? phoneNumber,
		string email,
		string? linkedInUrl,
		Guid supplierId) : base(id)
	{
		Name = name;
		PictureUrl = pictureUrl;
		PhoneNumber = phoneNumber;
		Email = email;
		LinkedInUrl = linkedInUrl;
		SupplierId = supplierId;
		AccountManagerDeals = new Collection<AccountManagerDeal>();
	}

}
