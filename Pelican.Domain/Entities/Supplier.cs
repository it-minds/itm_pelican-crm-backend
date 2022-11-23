using Pelican.Domain.Primitives;

namespace Pelican.Domain.Entities;

public class Supplier : Entity, ITimeTracked
{
	public long HubSpotId { get; set; }

	public string RefreshToken { get; set; }

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
	public void SetName(string newName)
	{
		Name = newName.Length > StringLengths.Name ? newName.Substring(0, StringLengths.Name - 3) + ("...") : newName;
	}
	public void SetPhoneNumber(string newPhoneNumber)
	{
		PhoneNumber = newPhoneNumber.Length > StringLengths.DealDescription ? newPhoneNumber.Substring(0, StringLengths.DealDescription - 3) + ("...") : newPhoneNumber;
	}
	public void SetEmail(string newEmail)
	{
		Email = newEmail.Length > StringLengths.Email ? newEmail.Substring(0, StringLengths.Email - 3) + ("...") : newEmail;
	}
	public void SetPictureUrl(string newPictureUrl)
	{
		PictureUrl = newPictureUrl.Length > StringLengths.Url ? newPictureUrl.Substring(0, StringLengths.Url - 3) + ("...") : newPictureUrl;
	}
	public void SetLinkedInUrl(string newLinkedInUrl)
	{
		LinkedInUrl = newLinkedInUrl.Length > StringLengths.Url ? newLinkedInUrl.Substring(0, StringLengths.Url - 3) + ("...") : newLinkedInUrl;
	}
	public void SetWebsiteUrl(string newWebsiteUrl)
	{
		WebsiteUrl = newWebsiteUrl.Length > StringLengths.Url ? newWebsiteUrl.Substring(0, StringLengths.Url - 3) + ("...") : newWebsiteUrl;
	}
}
