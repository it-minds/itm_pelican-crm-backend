using Bogus;
using Pelican.Application.Abstractions.Data;
using Pelican.Domain;
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
			.RuleFor(e => e.SourceId, f => f.Random.Guid().ToString())
			.RuleFor(e => e.SourceUserId, f => f.Random.Long(1))
			.RuleFor(e => e.Id, f => f.Random.Guid())
			.RuleFor(e => e.Source, () => new Random().Next(2) <= 1 ? Sources.HubSpot : Sources.Pipedrive);
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
			.RuleFor(e => e.SourceId, f => f.Random.Long(1))
			.RuleFor(e => e.RefreshToken, f => f.Random.Guid().ToString())
			.RuleFor(e => e.Id, f => f.Random.Guid())
			.RuleFor(e => e.PipedriveDomain, f => f.Internet.Url().OrNull(f, 0.0f))
			.RuleFor(e => e.OfficeLocation, f => f.Address.City().OrNull(f, 0.0f))
			.RuleFor(e => e.Source, () => new Random().Next(2) == 1 ? Sources.HubSpot : Sources.Pipedrive);
		return faker.Generate(count);
	}

	public IEnumerable<Deal> DealFaker(int count, IQueryable<Client> clients, IQueryable<AccountManager> accountManagers)
	{
		var faker = new Faker<Deal>().UseSeed(1339);
		faker
			.RuleFor(e => e.SourceId, f => f.Random.Guid().ToString())
			.RuleFor(e => e.SourceOwnerId, f => f.PickRandom<AccountManager>(accountManagers).SourceId.OrNull(f, 0.0f))
			.RuleFor(e => e.DealStatus, f => f.PickRandom<DealStatus>().ToString().OrNull(f, 0.0f))
			.RuleFor(e => e.EndDate, f => f.Date.Future().Ticks.OrNull(f, 0.0f))
			.RuleFor(e => e.StartDate, f => f.Date.Future().Ticks.OrNull(f, 0.0f))
			.RuleFor(e => e.Id, f => f.Random.Guid())
			.RuleFor(e => e.LastContactDate, f => f.Date.Future().Ticks.OrNull(f, 0.0f))
			.RuleFor(e => e.ClientId, f => f.PickRandom<Client>(clients).Id)
			.RuleFor(e => e.Client, f => f.PickRandom<Client>(clients))
			.RuleFor(e => e.Name, f => f.Lorem.Sentences(f.Random.Int(1, 4)).OrNull(f, 0.0f))
			.RuleFor(e => e.Description, f => f.Lorem.Sentences(f.Random.Int(1, 10)).OrNull(f, 0.0f))
			.RuleFor(e => e.Source, () => new Random().Next(2) == 1 ? Sources.HubSpot : Sources.Pipedrive);
		return faker.Generate(count);
	}


	public IEnumerable<Client> ClientFaker(int count)
	{
		var faker = new Faker<Client>().UseSeed(1341);
		faker
			.RuleFor(e => e.Name, f => f.Company.CompanyName())
			.RuleFor(e => e.PictureUrl, f => f.Image.PicsumUrl().OrNull(f, 0.0f))
			.RuleFor(e => e.OfficeLocation, f => f.Address.City().OrNull(f, 0.0f))
			.RuleFor(e => e.SourceId, f => f.Random.Guid().ToString())
			.RuleFor(e => e.Website, f => f.Internet.Url().OrNull(f, 0.0f))
			.RuleFor(e => e.Source, () => new Random().Next(2) == 1 ? Sources.HubSpot : Sources.Pipedrive)
			.RuleFor(e => e.Id, f => f.Random.Guid());
		return faker.Generate(count);
	}

	public IEnumerable<Contact> ContactFaker(int count, IQueryable<AccountManager> accountManagers)
	{
		var faker = new Faker<Contact>().UseSeed(1342);
		faker
			.RuleFor(e => e.FirstName, f => f.Name.FirstName(f.Person.Gender).OrNull(f, 0.0f))
			.RuleFor(e => e.LastName, f => f.Name.LastName(f.Person.Gender).OrNull(f, 0.0f))
			.RuleFor(e => e.Email, f => f.Person.Email.OrNull(f, 0.0f))
			.RuleFor(e => e.PhoneNumber, f => f.Phone.PhoneNumber().OrNull(f, 0.0f))
			.RuleFor(e => e.JobTitle, f => f.Name.JobTitle().OrNull(f, 0.0f))
			.RuleFor(e => e.SourceId, f => f.Random.Guid().ToString())
			.RuleFor(e => e.SourceOwnerId, f => f.PickRandom<AccountManager>(accountManagers).SourceId.OrNull(f, 0.0f))
			.RuleFor(e => e.Id, f => f.Random.Guid())
			.RuleFor(e => e.Source, () => new Random().Next(2) == 1 ? Sources.HubSpot : Sources.Pipedrive);
		return faker.Generate(count);
	}

	public IEnumerable<AccountManagerDeal> AccountManagerDealFaker(IQueryable<AccountManager> accountManagers, IQueryable<Deal> deals)
		=> new Faker<AccountManagerDeal>()
			.UseSeed(1343)
			.RuleFor(e => e.DealId, f => f.PickRandom<Deal>(deals).Id)
			.RuleFor(e => e.AccountManagerId, f => f.PickRandom<AccountManager>(accountManagers).Id)
			.RuleFor(e => e.IsActive, () => new Random().Next(100) <= 50)
			.RuleFor(e => e.Id, f => f.Random.Guid())
			.RuleFor(e => e.SourceAccountManagerId, f => f.PickRandom<AccountManager>(accountManagers).SourceId)
			.RuleFor(e => e.SourceDealId, f => f.PickRandom<Deal>(deals).SourceId)
			.Generate(accountManagers.Count());

	public IEnumerable<DealContact> DealContactFaker(IQueryable<Deal> deals, IQueryable<Contact> contacts)
		=> new Faker<DealContact>()
			.UseSeed(1344)
			.RuleFor(e => e.DealId, f => f.PickRandom<Deal>(deals).Id)
			.RuleFor(e => e.ContactId, f => f.PickRandom<Contact>(contacts).Id)
			.RuleFor(e => e.IsActive, () => new Random().Next(100) <= 50)
			.RuleFor(e => e.Id, f => f.Random.Guid())
			.RuleFor(e => e.SourceContactId, f => f.PickRandom<Contact>(contacts).SourceId)
			.RuleFor(e => e.SourceDealId, f => f.PickRandom<Deal>(deals).SourceId)
			.Generate(deals.Count());

	public IEnumerable<ClientContact> ClientContactFaker(IQueryable<Client> clients, IQueryable<Contact> contacts)
		=> new Faker<ClientContact>()
			.UseSeed(1345)
			.RuleFor(e => e.ClientId, f => f.PickRandom<Client>(clients).Id)
			.RuleFor(e => e.ContactId, f => f.PickRandom<Contact>(contacts).Id)
			.RuleFor(e => e.IsActive, () => new Random().Next(100) <= 50)
			.RuleFor(e => e.Id, f => f.Random.Guid())
			.RuleFor(e => e.SourceClientId, f => f.PickRandom<Client>(clients).SourceId)
			.RuleFor(e => e.SourceContactId, f => f.PickRandom<Contact>(contacts).SourceId)
			.Generate(clients.Count());
}
