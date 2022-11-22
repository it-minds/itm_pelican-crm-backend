using Bogus;
using Pelican.Application.Common.Interfaces;
using Pelican.Domain.Entities;
using Pelican.Domain.Enums;

namespace Pelican.Application;
public class PelicanBogusFaker : IPelicanBogusFaker
{
	public PelicanBogusFaker() { }
	//Creates as many dummy suppliers as inputted using rules setup here these are used to seed an empty database
	public IEnumerable<AccountManager> AccountManagerFaker(int count, IQueryable<Supplier> suppliers)
	{
		var faker = new Faker<AccountManager>().UseSeed(1337);
		faker
			.RuleFor(e => e.FirstName, f => f.Name.FirstName(f.Person.Gender))
			.RuleFor(e => e.LastName, f => f.Name.LastName(f.Person.Gender))
			.RuleFor(e => e.PictureUrl, f => f.Image.PicsumUrl().OrNull(f, 0.0f))
			.RuleFor(e => e.PhoneNumber, f => f.Phone.PhoneNumber().OrNull(f, 0.0f))
			.RuleFor(e => e.Email, f => f.Person.Email)
			.RuleFor(e => e.LinkedInUrl, f => f.Internet.Url().OrNull(f, 0.0f))
			.RuleFor(e => e.Supplier, f => f.PickRandom<Supplier>(suppliers))
			.RuleFor(e => e.HubSpotId, f => f.Random.Guid().ToString())
			.RuleFor(e => e.HubSpotUserId, f => f.Random.Long(1))
			.RuleFor(e => e.Id, f => f.Random.Guid());
		return faker.Generate(count);
	}
	public IEnumerable<Supplier> SupplierFaker(int count)
	{
		var faker = new Faker<Supplier>().UseSeed(1338);
		faker
			.RuleFor(e => e.Name, f => f.Company.CompanyName().OrNull(f, 0.0f))
			.RuleFor(e => e.PictureUrl, f => f.Image.PicsumUrl().OrNull(f, 0.0f))
			.RuleFor(e => e.PhoneNumber, f => f.Phone.PhoneNumber().OrNull(f, 0.0f))
			.RuleFor(e => e.Email, f => f.Person.Email.OrNull(f, 0.0f))
			.RuleFor(e => e.LinkedInUrl, f => f.Internet.Url().OrNull(f, 0.0f))
			.RuleFor(e => e.WebsiteUrl, f => f.Internet.Url().OrNull(f, 0.0f))
			.RuleFor(e => e.HubSpotId, f => f.Random.Long(1))
			.RuleFor(e => e.RefreshToken, f => f.Random.Guid().ToString())
			.RuleFor(e => e.Id, f => f.Random.Guid());
		return faker.Generate(count);
	}
	public IEnumerable<Deal> DealFaker(int count, IQueryable<Client> clients, IQueryable<AccountManager> accountManagers)
	{
		var faker = new Faker<Deal>().UseSeed(1339);
		faker
			.RuleFor(e => e.HubSpotId, f => f.Random.Guid().ToString())
			.RuleFor(e => e.HubSpotOwnerId, f => f.PickRandom<AccountManager>(accountManagers).HubSpotId.OrNull(f, 0.0f))
			.RuleFor(e => e.DealStatus, f => f.PickRandom<DealStatus>().ToString().OrNull(f, 0.0f))
			.RuleFor(e => e.EndDate, f => f.Date.Past().OrNull(f, 0.0f))
			.RuleFor(e => e.StartDate, f => f.Date.Future().OrNull(f, 0.0f))
			.RuleFor(e => e.Id, f => f.Random.Guid())
			.RuleFor(e => e.LastContactDate, f => f.Date.Past().OrNull(f, 0.0f))
			.RuleFor(e => e.ClientId, f => f.PickRandom<Client>(clients).Id)
			.RuleFor(e => e.Client, f => f.PickRandom<Client>(clients));
		return faker.Generate(count);
	}
	public IEnumerable<Location> LocationFaker(int count, IQueryable<Supplier> suppliers)
	{
		var faker = new Faker<Location>().UseSeed(1340);
		faker
			.RuleFor(e => e.CityName, f => f.Address.City())
			.RuleFor(e => e.Id, f => f.Random.Guid())
			.RuleFor(e => e.SupplierId, f => f.PickRandom<Supplier>(suppliers).Id)
			.RuleFor(e => e.Supplier, f => f.PickRandom<Supplier>(suppliers));
		return faker.Generate(count);
	}
	public IEnumerable<Client> ClientFaker(int count, IQueryable<Location> locations)
	{
		var faker = new Faker<Client>().UseSeed(1341);
		faker
			.RuleFor(e => e.Name, f => f.Name.FullName(f.Person.Gender))
			.RuleFor(e => e.PictureUrl, f => f.Image.PicsumUrl().OrNull(f, 0.0f))
			.RuleFor(e => e.OfficeLocation, f => f.PickRandom<Location>(locations).CityName.OrNull(f, 0.0f))
			.RuleFor(e => e.HubSpotId, f => f.Random.Guid().ToString())
			.RuleFor(e => e.Website, f => f.Internet.Url().OrNull(f, 0.0f))
			.RuleFor(e => e.Id, f => f.Random.Guid());
		return faker.Generate(count);
	}
	public IEnumerable<Contact> ContactFaker(int count, IQueryable<AccountManager> accountManagers)
	{
		var faker = new Faker<Contact>().UseSeed(1342);
		faker
			.RuleFor(e => e.Firstname, f => f.Name.FirstName(f.Person.Gender))
			.RuleFor(e => e.Lastname, f => f.Name.LastName(f.Person.Gender))
			.RuleFor(e => e.Email, f => f.Person.Email.OrNull(f, 0.0f))
			.RuleFor(e => e.PhoneNumber, f => f.Phone.PhoneNumber().OrNull(f, 0.0f))
			.RuleFor(e => e.JobTitle, f => f.Name.JobTitle().OrNull(f, 0.0f))
			.RuleFor(e => e.HubSpotId, f => f.Random.Guid().ToString())
			.RuleFor(e => e.HubSpotOwnerId, f => f.PickRandom<AccountManager>(accountManagers).HubSpotId.OrNull(f, 0.0f))
			.RuleFor(e => e.Id, f => f.Random.Guid());
		return faker.Generate(count);
	}
	public IEnumerable<AccountManagerDeal> AccountManagerDealFaker(IQueryable<AccountManager> accountManagers, IQueryable<Deal> deals)
	{
		Random gen = new Random();
		int prob = gen.Next(100);
		var faker = new Faker<AccountManagerDeal>().UseSeed(1343);
		faker
			.RuleFor(e => e.DealId, f => f.PickRandom<Deal>(deals).Id)
			.RuleFor(e => e.AccountManagerId, f => f.PickRandom<AccountManager>(accountManagers).Id)
			.RuleFor(e => e.IsActive, prob <= 20)
			.RuleFor(e => e.Id, f => f.Random.Guid())
			.RuleFor(e => e.HubSpotAccountManagerId, f => f.PickRandom<AccountManager>(accountManagers).HubSpotId)
			.RuleFor(e => e.HubSpotDealId, f => f.PickRandom<Deal>(deals).HubSpotId);
		return faker.Generate(accountManagers.Count());
	}
	public IEnumerable<DealContact> DealContactFaker(IQueryable<Deal> deals, IQueryable<Contact> contacts)
	{
		Random gen = new Random();
		int prob = gen.Next(100);
		var faker = new Faker<DealContact>().UseSeed(1344);
		faker
			.RuleFor(e => e.DealId, f => f.PickRandom<Deal>(deals).Id)
			.RuleFor(e => e.ContactId, f => f.PickRandom<Contact>(contacts).Id)
			.RuleFor(e => e.IsActive, prob <= 20)
			.RuleFor(e => e.Id, f => f.Random.Guid())
			.RuleFor(e => e.HubSpotContactId, f => f.PickRandom<Contact>(contacts).HubSpotId)
			.RuleFor(e => e.HubSpotDealId, f => f.PickRandom<Deal>(deals).HubSpotId);
		return faker.Generate(deals.Count());
	}
	public IEnumerable<ClientContact> ClientContactFaker(IQueryable<Client> clients, IQueryable<Contact> contacts)
	{
		Random gen = new Random();
		int prob = gen.Next(100);
		var faker = new Faker<ClientContact>().UseSeed(1345);
		faker
			.RuleFor(e => e.ClientId, f => f.PickRandom<Client>(clients).Id)
			.RuleFor(e => e.ContactId, f => f.PickRandom<Contact>(contacts).Id)
			.RuleFor(e => e.IsActive, prob >= 20)
			.RuleFor(e => e.Id, f => f.Random.Guid())
			.RuleFor(e => e.HubSpotClientId, f => f.PickRandom<Client>(clients).HubSpotId)
			.RuleFor(e => e.HubSpotContactId, f => f.PickRandom<Contact>(contacts).HubSpotId);
		return faker.Generate(clients.Count());
	}
}
