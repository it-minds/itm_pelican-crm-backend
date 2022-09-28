using Pelican.Domain.Primitives;

namespace Pelican.Domain.Entities;
public class AccountManager : Entity, ITimeTracked
{
	public string Name { get; set; }
	public string PictureUrl { get; set; }
	public string PhoneNumber { get; set; }
	public string Email { get; set; }
	public string LinkedIn { get; set; }
	public Supplier Supplier { get; set; }
	public ICollection<AccountManagerDeal> AccountManagerDeals { get; set; }
	public long CreatedAt { get; set; }
	public long? LastUpdatedAt { get; set; }

	public AccountManager(Guid id, string name,
		string pictureUrl,
		string phoneNumber,
		string email,
		string linkedIn,
		Supplier supplier,
		ICollection<AccountManagerDeal> accountManagerDeals) : base(id)
	{
		Name = name;
		PictureUrl = pictureUrl;
		PhoneNumber = phoneNumber;
		Email = email;
		LinkedIn = linkedIn;
		Supplier = supplier;
		AccountManagerDeals = accountManagerDeals;
	}
}
