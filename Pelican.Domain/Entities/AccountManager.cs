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
	public AccountManager(Guid id) : base(id) { }
	public AccountManager() { }
}
