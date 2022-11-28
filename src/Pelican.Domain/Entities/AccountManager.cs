﻿using Pelican.Domain.Primitives;

namespace Pelican.Domain.Entities;
public class AccountManager : Entity, ITimeTracked
{
	private string _firstName = string.Empty;
	private string _lastName = string.Empty;
	private string _email = string.Empty;
	private string? _phoneNumber;
	private string? _linkedInUrl;

	public AccountManager(Guid id) : base(id) { }

	public AccountManager() { }

	public string HubSpotId { get; set; } = string.Empty;

	public long HubSpotUserId { get; set; }

	public string FirstName
	{
		get => _firstName;
		set
		{
			_firstName = value.Length > StringLengths.Name
				? value.Substring(0, StringLengths.Name - 3) + ("...")
				: value;
		}
	}
	public string LastName
	{
		get => _lastName;
		set
		{
			_lastName = value.Length > StringLengths.Name
				? value.Substring(0, StringLengths.Name - 3) + ("...")
				: value;
		}
	}

	public string Email
	{
		get => _email;
		set
		{
			_email = value.Length > StringLengths.Email
				? value.Substring(0, StringLengths.Email - 3) + ("...")
				: value;
		}
	}

	public string? PhoneNumber
	{
		get => _phoneNumber;
		set
		{
			_phoneNumber = value!.Length > StringLengths.PhoneNumber
				? value.Substring(0, StringLengths.PhoneNumber - 3) + ("...")
				: value;
		}
	}

	public string? PictureUrl { get; set; }

	public string? LinkedInUrl
	{
		get => _linkedInUrl;
		set
		{
			_linkedInUrl = value!.Length > StringLengths.Url
				? value.Substring(0, StringLengths.Url - 3) + ("...")
				: value;
		}
	}

	public Guid SupplierId { get; set; }

	public Supplier Supplier { get; set; }

	public ICollection<AccountManagerDeal> AccountManagerDeals { get; set; } = new List<AccountManagerDeal>();

	public long CreatedAt { get; set; }

	public long? LastUpdatedAt { get; set; }
}