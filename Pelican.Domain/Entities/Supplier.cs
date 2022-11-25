﻿using Pelican.Domain.Primitives;

namespace Pelican.Domain.Entities;

public class Supplier : Entity, ITimeTracked
{
	private string? _name;
	private string? _phoneNumber;
	private string? _email;
	private string? _linkedInUrl;
	private string? _websiteUrl;

	public Supplier(Guid id) : base(id) { }

	public Supplier() { }

	public long HubSpotId { get; set; }

	public string RefreshToken { get; set; } = string.Empty;


	public string? Name
	{
		get => _name;
		set
		{
			_name = value!.Length > StringLengths.Name
				? value.Substring(0, StringLengths.Name - 3) + ("...")
				: value;
		}
	}
	public string? PictureUrl { get; set; }


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
	public string? Email
	{
		get => _email;
		set
		{
			_email = value!.Length > StringLengths.Email
				? value.Substring(0, StringLengths.Email - 3) + ("...")
				: value;
		}
	}

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
	public string? WebsiteUrl
	{
		get => _websiteUrl;
		set
		{
			_websiteUrl = value!.Length > StringLengths.Url
				? value.Substring(0, StringLengths.Url - 3) + ("...")
				: value;
		}
	}

	public ICollection<Location> OfficeLocations { get; set; } = new List<Location>();

	public ICollection<AccountManager> AccountManagers { get; set; } = new List<AccountManager>();


	public long CreatedAt { get; set; }

	public long? LastUpdatedAt { get; set; }
}
