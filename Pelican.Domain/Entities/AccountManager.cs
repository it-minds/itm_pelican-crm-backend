using Pelican.Domain.Primitives;

namespace Pelican.Domain.Entities;
public class AccountManager : Entity, ITimeTracked
{
	public string HubSpotId { get; set; } = string.Empty;

	public long HubSpotUserId { get; set; }

	private string _firstName = string.Empty;
	public string FirstName
	{
		get => _firstName;
		set
		{
			_firstName = value.Length > StringLengths.Name ? value.Substring(0, StringLengths.Name - 3) + ("...") : value;
		}
	}
	private string _lastName = string.Empty;
	public string LastName
	{
		get => _lastName;
		set
		{
			_lastName = value.Length > StringLengths.Name ? value.Substring(0, StringLengths.Name - 3) + ("...") : value;
		}
	}
	private string _email = string.Empty;
	public string Email
	{
		get => _email;
		set
		{
			_email = value.Length > StringLengths.Email ? value.Substring(0, StringLengths.Email - 3) + ("...") : value;
		}
	}
	private string? _phoneNumber;
	public string? PhoneNumber
	{
		get => _phoneNumber;
		set
		{
			_phoneNumber = value!.Length > StringLengths.PhoneNumber ? value.Substring(0, StringLengths.PhoneNumber - 3) + ("...") : value;
		}
	}
	private string? _pictureUrl;
	public string? PictureUrl
	{
		get => _pictureUrl;
		set
		{
			_pictureUrl = value!.Length > StringLengths.Url ? value.Substring(0, StringLengths.Url - 3) + ("...") : value;
		}
	}
	private string? _linkedInUrl;
	public string? LinkedInUrl
	{
		get => _linkedInUrl;
		set
		{
			_linkedInUrl = value!.Length > StringLengths.Url ? value.Substring(0, StringLengths.Url - 3) + ("...") : value;
		}
	}

	public Guid SupplierId { get; set; }

	public Supplier Supplier { get; set; }

	public ICollection<AccountManagerDeal> AccountManagerDeals { get; set; } = new List<AccountManagerDeal>();

	public long CreatedAt { get; set; }

	public long? LastUpdatedAt { get; set; }


	public AccountManager(Guid id) : base(id) { }

	public AccountManager() { }
}
