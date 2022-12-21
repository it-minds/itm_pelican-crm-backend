using Pelican.Domain.Extensions;
using Pelican.Domain.Primitives;

namespace Pelican.Domain.Entities;
public class AccountManager : Entity, ITimeTracked
{
	private string _firstName = string.Empty;
	private string _lastName = string.Empty;
	private string _email = string.Empty;
	private string? _phoneNumber;
	private string? _linkedInUrl;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
	public AccountManager() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

	public string SourceId { get; set; } = string.Empty;

	public string Source { get; set; } = string.Empty;

	public long SourceUserId { get; set; }

	public string FirstName
	{
		get => _firstName;
		set => _firstName = value.CheckAndShortenExceedingString(StringLengths.Name);
	}
	public string LastName
	{
		get => _lastName;
		set => _lastName = value.CheckAndShortenExceedingString(StringLengths.Name);
	}

	public string Email
	{
		get => _email;
		set => _email = value.CheckAndShortenExceedingString(StringLengths.Email);
	}

	public string? PhoneNumber
	{
		get => _phoneNumber;
		set => _phoneNumber = value?.CheckAndShortenExceedingString(StringLengths.PhoneNumber);
	}

	public string? PictureUrl { get; set; }

	public string? LinkedInUrl
	{
		get => _linkedInUrl;
		set => _linkedInUrl = value?.CheckAndShortenExceedingString(StringLengths.Url);
	}

	public Guid SupplierId { get; set; }

	public Supplier Supplier { get; set; }

	public ICollection<AccountManagerDeal> AccountManagerDeals { get; set; } = new List<AccountManagerDeal>();

	public long CreatedAt { get; set; }

	public long? LastUpdatedAt { get; set; }
}
