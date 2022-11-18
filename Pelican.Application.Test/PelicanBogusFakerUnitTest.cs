using Pelican.Application.Abstractions.Data;
using Pelican.Domain.Entities;
using Xunit;

namespace Pelican.Application.Test;
public class PelicanBogusFakerUnitTest
{
	private readonly IPelicanBogusFaker uut;
	public PelicanBogusFakerUnitTest()
	{
		uut = new PelicanBogusFaker();
	}

	[Theory]
	[InlineData(1)]
	public void SupplierFaker_Id_HubSpotId_RefreshToken_NotNullEmptyOrWhiteSpace(int param)
	{
		//Arrange
		//Act
		IEnumerable<Supplier> result = uut.SupplierFaker(param);
		//Assert
		Assert.Single(result);
		Assert.All(result,
			item => Assert.True(Guid.TryParse(item.Id.ToString(), out _)));
		Assert.All(result,
			item => Assert.NotEqual(default, item.HubSpotId));
		Assert.All(result,
			item => Assert.False(string.IsNullOrWhiteSpace(item.RefreshToken)));
	}

	[Theory]
	[InlineData(1)]
	public void AccountManagerFaker_Id_HubSpotId_SupplierId_Supplier_FirstName_LastName_Email_NotNullEmptyOrWhiteSpace(int param)
	{
		//Arrange
		var supplier = uut.SupplierFaker(param).AsQueryable();
		//Act
		IEnumerable<AccountManager> result = uut.AccountManagerFaker(param, supplier);
		//Assert
		Assert.Single(result);
		Assert.All(result,
			item => Assert.True(Guid.TryParse(item.Id.ToString(), out _)));
		Assert.All(result,
			item => Assert.False(string.IsNullOrWhiteSpace(item.HubSpotId)));
		Assert.All(result,
			item => Assert.True(Guid.TryParse(item.SupplierId.ToString(), out _)));
		Assert.All(result,
			item => Assert.True(Guid.TryParse(item.SupplierId.ToString(), out _)));
		Assert.All(result,
			item => Assert.NotNull(item.Supplier));
		Assert.All(result,
			item => Assert.False(string.IsNullOrWhiteSpace(item.FirstName)));
		Assert.All(result,
			item => Assert.False(string.IsNullOrWhiteSpace(item.LastName)));
		Assert.All(result,
			item => Assert.False(string.IsNullOrWhiteSpace(item.Email)));
	}

	[Theory]
	[InlineData(1)]
	public void AccountManagerDealFaker_Id_DealId_AccountManagerId_HubSpotDealId_HubSpotAccountManagerId_IsActive_NotNullEmptyOrWhiteSpace(int param)
	{
		//Arrange
		var supplier = uut.SupplierFaker(param).AsQueryable();
		var accountManager = uut.AccountManagerFaker(param, supplier).AsQueryable();
		var location = uut.LocationFaker(param, supplier).AsQueryable();
		var client = uut.ClientFaker(param, location).AsQueryable();
		var deal = uut.DealFaker(param, client, accountManager).AsQueryable();
		//Act
		IEnumerable<AccountManagerDeal> result = uut.AccountManagerDealFaker(accountManager, deal);
		//Assert
		Assert.Single(result);
		Assert.All(result,
			item => Assert.True(Guid.TryParse(item.Id.ToString(), out _)));
		Assert.All(result,
			item => Assert.True(Guid.TryParse(item.DealId.ToString(), out _)));
		Assert.All(result,
			item => Assert.True(Guid.TryParse(item.AccountManagerId.ToString(), out _)));
		Assert.All(result,
			item => Assert.False(string.IsNullOrWhiteSpace(item.HubSpotAccountManagerId)));
		Assert.All(result,
			item => Assert.False(string.IsNullOrWhiteSpace(item.HubSpotDealId)));
	}

	[Theory]
	[InlineData(1)]
	public void ClientFaker_Id_HubSpotId_Name_NotNullEmptyOrWhiteSpace(int param)
	{
		//Arrange
		var supplier = uut.SupplierFaker(param).AsQueryable();
		var location = uut.LocationFaker(param, supplier).AsQueryable();
		//Act
		IEnumerable<Client> result = uut.ClientFaker(param, location);
		//Assert
		Assert.Single(result);
		Assert.All(result,
			item => Assert.True(Guid.TryParse(item.Id.ToString(), out _)));
		Assert.All(result,
			item => Assert.False(string.IsNullOrWhiteSpace(item.HubSpotId)));
		Assert.All(result,
			item => Assert.False(string.IsNullOrWhiteSpace(item.Name)));
	}

	[Theory]
	[InlineData(1)]
	public void ClientContactFaker_Id_ClientId_ContactId_HubSpotClientId_HubSpotContactId_NotNullEmptyOrWhiteSpace(int param)
	{
		//Arrange
		var supplier = uut.SupplierFaker(param).AsQueryable();
		var location = uut.LocationFaker(param, supplier).AsQueryable();
		var client = uut.ClientFaker(param, location).AsQueryable();
		var accountManager = uut.AccountManagerFaker(param, supplier).AsQueryable();
		var contact = uut.ContactFaker(param, accountManager).AsQueryable();
		//Act
		IEnumerable<ClientContact> result = uut.ClientContactFaker(client, contact);
		//Assert
		Assert.Single(result);
		Assert.All(result,
			item => Assert.True(Guid.TryParse(item.Id.ToString(), out _)));
		Assert.All(result,
			item => Assert.True(Guid.TryParse(item.ClientId.ToString(), out _)));
		Assert.All(result,
			item => Assert.True(Guid.TryParse(item.ContactId.ToString(), out _)));
		Assert.All(result,
			item => Assert.False(string.IsNullOrWhiteSpace(item.HubSpotClientId)));
		Assert.All(result,
			item => Assert.False(string.IsNullOrWhiteSpace(item.HubSpotContactId)));
	}

	[Theory]
	[InlineData(1)]
	public void ContactFaker_Id_Firstname_Lastname_ClientContacts_HubSpotId_NotNullEmptyOrWhiteSpace(int param)
	{
		//Arrange
		var supplier = uut.SupplierFaker(param).AsQueryable();
		var accountManager = uut.AccountManagerFaker(param, supplier).AsQueryable();
		//Act
		IEnumerable<Contact> result = uut.ContactFaker(param, accountManager);
		//Assert
		Assert.Single(result);
		Assert.All(result,
			item => Assert.True(Guid.TryParse(item.Id.ToString(), out _)));
		Assert.All(result,
			item => Assert.False(string.IsNullOrWhiteSpace(item.Firstname)));
		Assert.All(result,
			item => Assert.False(string.IsNullOrWhiteSpace(item.Lastname)));
		Assert.All(result,
			item => Assert.NotNull(item.ClientContacts));
		Assert.All(result,
			item => Assert.False(string.IsNullOrWhiteSpace(item.HubSpotId)));
	}

	[Theory]
	[InlineData(1)]
	public void DealFaker_Id_AccountManagerDeals_HubSpotId_HubSpotOwnerId_NotNullEmptyOrWhiteSpace(int param)
	{
		//Arrange
		var supplier = uut.SupplierFaker(param).AsQueryable();
		var location = uut.LocationFaker(param, supplier).AsQueryable();
		var client = uut.ClientFaker(param, location).AsQueryable();
		var accountManager = uut.AccountManagerFaker(param, supplier).AsQueryable();
		var contact = uut.ContactFaker(param, accountManager).AsQueryable();
		//Act
		IEnumerable<Deal> result = uut.DealFaker(param, client, accountManager);
		//Assert
		Assert.Single(result);
		Assert.All(result,
			item => Assert.True(Guid.TryParse(item.Id.ToString(), out _)));
		Assert.All(result,
			item => Assert.NotNull(item.AccountManagerDeals));
		Assert.All(result,
			item => Assert.False(string.IsNullOrWhiteSpace(item.HubSpotId)));
	}

	[Theory]
	[InlineData(1)]
	public void DealContactFaker_Id_DealId_ContactId_HubSpotDealId_HubSpotContactId_IsActive_NotNullEmptyOrWhiteSpace(int param)
	{
		//Arrange
		var supplier = uut.SupplierFaker(param).AsQueryable();
		var location = uut.LocationFaker(param, supplier).AsQueryable();
		var client = uut.ClientFaker(param, location).AsQueryable();
		var accountManager = uut.AccountManagerFaker(param, supplier).AsQueryable();
		var contact = uut.ContactFaker(param, accountManager).AsQueryable();
		var deal = uut.DealFaker(param, client, accountManager).AsQueryable();
		//Act
		IEnumerable<DealContact> result = uut.DealContactFaker(deal, contact);
		//Assert
		Assert.Single(result);
		Assert.All(result,
			item => Assert.True(Guid.TryParse(item.Id.ToString(), out _)));
		Assert.All(result,
			item => Assert.True(Guid.TryParse(item.DealId.ToString(), out _)));
		Assert.All(result,
			item => Assert.True(Guid.TryParse(item.ContactId.ToString(), out _)));
		Assert.All(result,
			item => Assert.NotEqual(default, item.HubSpotContactId.ToString()));
		Assert.All(result,
			item => Assert.False(string.IsNullOrWhiteSpace(item.HubSpotContactId)));
	}

	[Theory]
	[InlineData(1)]
	public void LocationFaker_Id_CityName_Supplier_SupplierId_NotNullEmptyOrWhiteSpace(int param)
	{
		//Arrange
		var supplier = uut.SupplierFaker(param).AsQueryable();
		//Act
		IEnumerable<Location> result = uut.LocationFaker(param, supplier);
		//Assert
		Assert.Single(result);
		Assert.All(result,
			item => Assert.True(Guid.TryParse(item.Id.ToString(), out _)));
		Assert.All(result,
			item => Assert.False(string.IsNullOrWhiteSpace(item.CityName)));
		Assert.All(result,
			item => Assert.NotNull(item.Supplier));
		Assert.All(result,
			item => Assert.True(Guid.TryParse(item.SupplierId.ToString(), out _))); ;
	}
}
