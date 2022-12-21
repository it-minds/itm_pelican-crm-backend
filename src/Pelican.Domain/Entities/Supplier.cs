using Pelican.Domain.Extensions;
using Pelican.Domain.Primitives;

namespace Pelican.Domain.Entities;

public class Supplier : Entity, ITimeTracked
{
	private string? _name;
	private string? _phoneNumber;
	private string? _email;
	private string? _linkedInUrl;
	private string? _websiteUrl;
	private string? _officeLocation;

	public Supplier() { }

	public string Source { get; set; } = string.Empty;

	public long SourceId { get; set; }

	public string RefreshToken { get; set; } = string.Empty;

	public string? Name
	{
		get => _name;
		set => _name = value?.CheckAndShortenExceedingString(StringLengths.Name);
	}
	public string? PictureUrl { get; set; }


	public string? PhoneNumber
	{
		get => _phoneNumber;
		set => _phoneNumber = value?.CheckAndShortenExceedingString(StringLengths.PhoneNumber);
	}
	public string? Email
	{
		get => _email;
		set => _email = value?.CheckAndShortenExceedingString(StringLengths.Email);
	}

	public string? LinkedInUrl
	{
		get => _linkedInUrl;
		set => _linkedInUrl = value?.CheckAndShortenExceedingString(StringLengths.Url);
	}
	public string? WebsiteUrl
	{
		get => _websiteUrl;
		set => _websiteUrl = value?.CheckAndShortenExceedingString(StringLengths.Url);
	}

	public string? OfficeLocation
	{
		get => _officeLocation;
		set => _officeLocation = value?.CheckAndShortenExceedingString(StringLengths.OfficeLocation);
	}

	public string? PipedriveDomain { get; set; }

	public ICollection<AccountManager> AccountManagers { get; set; } = new List<AccountManager>();

	public long CreatedAt { get; set; }

	public long? LastUpdatedAt { get; set; }
}
