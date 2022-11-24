using Pelican.Domain.Primitives;

namespace Pelican.Domain.Entities;

public class Supplier : Entity, ITimeTracked
{
	public long HubSpotId { get; set; }

	private string _refreshToken;
	public string RefreshToken
	{
		get => _refreshToken;
		set
		{
			_refreshToken = value!.Length > StringLengths.Token ? value.Substring(0, StringLengths.Token - 3) + ("...") : value;
		}
	}

	private string? _name;
	public string? Name
	{
		get => _name;
		set
		{
			_name = value!.Length > StringLengths.Name ? value.Substring(0, StringLengths.Name - 3) + ("...") : value;
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

	private string? _phoneNumber;
	public string? PhoneNumber
	{
		get => _phoneNumber;
		set
		{
			_phoneNumber = value!.Length > StringLengths.PhoneNumber ? value.Substring(0, StringLengths.PhoneNumber - 3) + ("...") : value;
		}
	}
	private string? _email;
	public string? Email
	{
		get => _email;
		set
		{
			_email = value!.Length > StringLengths.Email ? value.Substring(0, StringLengths.Email - 3) + ("...") : value;
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
	private string? _websiteUrl;
	public string? WebsiteUrl
	{
		get => _websiteUrl;
		set
		{
			_websiteUrl = value!.Length > StringLengths.Url ? value.Substring(0, StringLengths.Url - 3) + ("...") : value;
		}
	}

	public ICollection<Location> OfficeLocations { get; set; } = new List<Location>();

	public ICollection<AccountManager> AccountManagers { get; set; } = new List<AccountManager>();


	public long CreatedAt { get; set; }

	public long? LastUpdatedAt { get; set; }


	public Supplier(Guid id) : base(id) { }

	public Supplier() { }
}
