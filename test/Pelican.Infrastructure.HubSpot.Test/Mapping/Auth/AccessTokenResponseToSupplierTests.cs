using Castle.Core.Configuration;
using Pelican.Domain;
using Pelican.Infrastructure.HubSpot.Contracts.Responses.Auth;
using Pelican.Infrastructure.HubSpot.Mapping.Auth;
using Xunit;

namespace Pelican.Infrastructure.HubSpot.Test.Mapping.Auth;

public class AccessTokenResponseToSupplierTests
{
	[Fact]
	public void ToSupplier_ReturnCorrectProperties()
	{
		/// Arrange 
		const string HUBDOMAIN = "hubdomain";
		const long HUBID = 123;

		AccessTokenResponse response = new()
		{
			HubDomain = HUBDOMAIN,
			HubId = HUBID,
		};

		/// Act
		var result = response.ToSupplier();

		/// Assert
		Assert.Equal(HUBDOMAIN, result.WebsiteUrl);
		Assert.Equal(HUBDOMAIN, result.Name);
		Assert.Equal(HUBID, result.SourceId);
		Assert.Equal(Sources.HubSpot, result.Source);
	}

	[Fact]
	public void ToSupplier_DomainEndingRemovedInName()
	{
		/// Arrange 
		const string HUBDOMAIN = "hubdomain";

		AccessTokenResponse response = new()
		{
			HubDomain = $"{HUBDOMAIN}.com",
		};

		/// Act
		var result = response.ToSupplier();

		/// Assert
		Assert.Equal(HUBDOMAIN, result.Name);
	}

	[Fact]
	public void ToSupplier_DomainStartAndEndingRemovedInName()
	{
		/// Arrange 
		const string HUBDOMAIN = "hubdomain";

		AccessTokenResponse response = new()
		{
			HubDomain = $"www.{HUBDOMAIN}.com",
		};

		/// Act
		var result = response.ToSupplier();

		/// Assert
		Assert.Equal(HUBDOMAIN, result.Name);
	}

	[Fact]
	public void ToSupplier_NoDomainValue_NameNotSet()
	{
		/// Arrange 
		AccessTokenResponse response = new()
		{
			HubDomain = string.Empty,
		};

		/// Act
		var result = response.ToSupplier();

		/// Assert
		Assert.Equal(string.Empty, result.Name);
	}
}
