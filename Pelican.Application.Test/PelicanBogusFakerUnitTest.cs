using Pelican.Application.Common.Interfaces;
using Pelican.Domain.Entities;
using Xunit;

namespace Pelican.Application.Test;
public class PelicanBogusFakerUnitTest
{
	private IPelicanBogusFaker uut;
	public PelicanBogusFakerUnitTest()
	{
		uut = new PelicanBogusFaker();
	}
	[Fact]
	public void SupplierFaker_Id_HubSpotId_CreatedAt_RefreshToken_NotNull()
	{
		//Arrange
		//Act
		IEnumerable<Supplier> result = uut.SupplierFaker(1);
		//Assert
		Assert.Single(result);
		Assert.All(result,
			item => Assert.True(Guid.TryParse(item.Id.ToString(), out _)));
		Assert.All(result,
		item => Assert.NotNull(item.HubSpotId));
		Assert.All(result,
		item => Assert.NotNull(item.CreatedAt));
		Assert.All(result,
		item => Assert.NotNull(item.RefreshToken));
	}
	[Fact]
	public void AccountManagerFaker_Id_HubSpotId_CreatedAt_SupplierId_Supplier_FirstName_LastName_Email_NotNull()
	{
		//Arrange
		//Act
		IEnumerable<AccountManager> result = uut.AccountManagerFaker(1, uut.SupplierFaker(1).AsQueryable());
		//Assert
		Assert.Single(result);
		Assert.All(result,
			item => Assert.True(Guid.TryParse(item.Id.ToString(), out _)));
		Assert.All(result,
		item => Assert.NotNull(item.HubSpotId));
		Assert.All(result, item => Assert.NotNull(item.CreatedAt));
		Assert.All(result,
			item => Assert.True(Guid.TryParse(item.SupplierId.ToString(), out _)));
		Assert.All(result,
			item => Assert.True(Guid.TryParse(item.SupplierId.ToString(), out _)));
		Assert.All(result,
		item => Assert.NotNull(item.Supplier));
		Assert.All(result,
		item => Assert.NotNull(item.FirstName));
		Assert.All(result,
		item => Assert.NotNull(item.LastName));
		Assert.All(result,
		item => Assert.NotNull(item.Email));
	}
	[Fact]
	public void AccountManagerDealFaker_Id_CreatedAt_DealId_AccountManagerId_HubSpotDealId_HubSpotAccountManagerId_IsActive_NotNull()
	{
		//Arrange
		//Act
		IEnumerable<AccountManagerDeal> result = uut.AccountManagerDealFaker(
			uut.AccountManagerFaker(1, uut.SupplierFaker(1).AsQueryable()).AsQueryable(),
			uut.DealFaker(1, uut.ClientFaker(1, uut.LocationFaker(1, uut.SupplierFaker(1).AsQueryable()).AsQueryable()).AsQueryable(),
			uut.AccountManagerFaker(1, uut.SupplierFaker(1).AsQueryable()).AsQueryable()).AsQueryable());
		//Assert
		Assert.Single(result);
		Assert.All(result,
			item => Assert.True(Guid.TryParse(item.Id.ToString(), out _)));
		Assert.All(result,
		item => Assert.NotNull(item.CreatedAt));
		Assert.All(result,
			item => Assert.True(Guid.TryParse(item.DealId.ToString(), out _)));
		Assert.All(result,
			item => Assert.True(Guid.TryParse(item.AccountManagerId.ToString(), out _)));
		Assert.All(result,
		item => Assert.NotNull(item.HubSpotAccountManagerId));
		Assert.All(result,
		item => Assert.NotNull(item.HubSpotDealId));
		Assert.All(result,
		item => Assert.NotNull(item.IsActive));
	}
	[Fact]
	public void ClientFaker_Id_CreatedAt_HubSpotId_Name_NotNull()
	{
		//Arrange
		//Act
		IEnumerable<Client> result = uut.ClientFaker(1, uut.LocationFaker(1, uut.SupplierFaker(1).AsQueryable()).AsQueryable());
		//Assert
		Assert.Single(result);
		Assert.All(result,
			item => Assert.True(Guid.TryParse(item.Id.ToString(), out _)));
		Assert.All(result,
		item => Assert.NotNull(item.CreatedAt));
		Assert.All(result,
			item => Assert.NotNull(item.HubSpotId));
		Assert.All(result,
			item => Assert.NotNull(item.Name));
	}
	[Fact]
	public void ClientContactFaker_Id_CreatedAt_ClientId_ContactId_HubSpotClientId_HubSpotContactId_NotNull()
	{
		//Arrange
		//Act
		IEnumerable<ClientContact> result = uut.ClientContactFaker(uut.ClientFaker(1,
			uut.LocationFaker(1, uut.SupplierFaker(1).AsQueryable()).AsQueryable()).AsQueryable(),
			uut.ContactFaker(1, uut.AccountManagerFaker(1, uut.SupplierFaker(1).AsQueryable()).AsQueryable()).AsQueryable());
		//Assert
		Assert.Single(result);
		Assert.All(result,
			item => Assert.True(Guid.TryParse(item.Id.ToString(), out _)));
		Assert.All(result,
		item => Assert.NotNull(item.CreatedAt));
		Assert.All(result,
			item => Assert.True(Guid.TryParse(item.ClientId.ToString(), out _)));
		Assert.All(result,
			item => Assert.True(Guid.TryParse(item.ContactId.ToString(), out _)));
		Assert.All(result,
			item => Assert.True(Guid.TryParse(item.HubSpotClientId.ToString(), out _)));
		Assert.All(result,
			item => Assert.True(Guid.TryParse(item.HubspotContactId.ToString(), out _)));
		Assert.All(result,
		item => Assert.NotNull(item.IsActive));
	}
	[Fact]
	public void ContactFaker_Id_CreatedAt_Firstname_Lastname_ClientContacts_HubSpotId_NotNull()
	{
		//Arrange
		//Act
		IEnumerable<Contact> result = uut.ContactFaker(1, uut.AccountManagerFaker(1, uut.SupplierFaker(1).AsQueryable()).AsQueryable());
		//Assert
		Assert.Single(result);
		Assert.All(result,
			item => Assert.True(Guid.TryParse(item.Id.ToString(), out _)));
		Assert.All(result,
		item => Assert.NotNull(item.CreatedAt));
		Assert.All(result,
		item => Assert.NotNull(item.Firstname));
		Assert.All(result,
		item => Assert.NotNull(item.Lastname));
		Assert.All(result,
		item => Assert.NotNull(item.ClientContacts));
		Assert.All(result,
		item => Assert.NotNull(item.HubSpotId));
	}
	[Fact]
	public void DealFaker_Id_CreatedAt_AccountManagerDeals_HubSpotId_HubSpotOwnerId_NotNull()
	{
		//Arrange
		//Act
		IEnumerable<Deal> result = uut.DealFaker(1, uut.ClientFaker(1,
			uut.LocationFaker(1, uut.SupplierFaker(1).AsQueryable()).AsQueryable())
			.AsQueryable(), uut.AccountManagerFaker(1, uut.SupplierFaker(1).AsQueryable()).AsQueryable()).AsQueryable();
		//Assert
		Assert.Single(result);
		Assert.All(result,
			item => Assert.True(Guid.TryParse(item.Id.ToString(), out _)));
		Assert.All(result,
		item => Assert.NotNull(item.CreatedAt));
		Assert.All(result,
		item => Assert.NotNull(item.AccountManagerDeals));
		Assert.All(result,
		item => Assert.NotNull(item.HubSpotId));
		Assert.All(result,
		item => Assert.NotNull(item.HubSpotOwnerId));
	}
	[Fact]
	public void LocationFaker_Id_CreatedAt_AccountManagerDeals_HubSpotId_HubSpotOwnerId_NotNull()
	{
		//Arrange
		//Act
		IEnumerable<Location> result = uut.LocationFaker(1, uut.SupplierFaker(1).AsQueryable());
		//Assert
		Assert.Single(result);
		Assert.All(result,
			item => Assert.True(Guid.TryParse(item.Id.ToString(), out _)));
		Assert.All(result,
		item => Assert.NotNull(item.CreatedAt));
		Assert.All(result,
		item => Assert.NotNull(item.CityName));
		Assert.All(result,
		item => Assert.NotNull(item.Supplier));
		Assert.All(result,
			item => Assert.True(Guid.TryParse(item.SupplierId.ToString(), out _))); ;
	}
}
