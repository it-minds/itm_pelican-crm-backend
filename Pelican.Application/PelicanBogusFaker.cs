using Bogus;
using Pelican.Application.Common.Interfaces;
using Pelican.Domain.Entities;
using Pelican.Domain.Enums;
using Location = Pelican.Domain.Entities.Location;

namespace Pelican.Application;
public class PelicanBogusFaker : IPelicanBogusFaker
{
	public PelicanBogusFaker() { }

	public IEnumerable<AccountManager> AccountManagerFaker(int supplierCount, IQueryable<Supplier> suppliers)
	{
		var faker = new Faker<AccountManager>().UseSeed(1337);

		faker
			.RuleFor(e => e.Name, f => f.Name.FullName(f.Person.Gender))
			.RuleFor(e => e.PictureUrl, f => f.Image.PicsumUrl())
			.RuleFor(e => e.PhoneNumber, f => f.Phone.PhoneNumber())
			.RuleFor(e => e.Email, f => f.Person.Email)
			.RuleFor(e => e.LinkedInUrl, f => f.Internet.Url())
			.RuleFor(e => e.SupplierId, f => f.PickRandom<Supplier>(suppliers).Id)
			.RuleFor(e => e.Id, f => f.Random.Guid());
		return faker.Generate(supplierCount);
	}
	public IEnumerable<Supplier> SupplierFaker(int count, IQueryable<Location> locations)
	{
		var faker = new Faker<Supplier>().UseSeed(1338);
		faker
			.RuleFor(e => e.Name, f => f.Name.FullName(f.Person.Gender))
			.RuleFor(e => e.Email, f => f.Person.Email)
			.RuleFor(e => e.WebsiteUrl, f => f.Internet.Url())
			.RuleFor(e => e.LinkedInUrl, f => f.Internet.Url())
			.RuleFor(e => e.PhoneNumber, f => f.Phone.PhoneNumber())
			.RuleFor(e => e.PictureUrl, f => f.Image.PicsumUrl())
			.RuleFor(e => e.OfficeLocations, locations.ToList())
			.RuleFor(e => e.Id, f => f.Random.Guid());
		return faker.Generate(count);
	}
	public IEnumerable<Deal> DealFaker(int count, IQueryable<Client> clients)
	{
		var faker = new Faker<Deal>().UseSeed(1339);
		faker
			.RuleFor(e => e.Revenue, f => f.Random.Decimal() * 10000)
			.RuleFor(e => e.DealStatus, f => f.PickRandom<DealStatus>().ToString())
			.RuleFor(e => e.EndDate, f => new DateTime())
			.RuleFor(e => e.Id, f => f.Random.Guid())
			.RuleFor(e => e.ClientId, f => f.PickRandom<Client>(clients).Id);
		return faker.Generate(count);
	}
	public IEnumerable<Location> LocationFaker(int count)
	{
		var faker = new Faker<Location>().UseSeed(1340);
		faker
			.RuleFor(e => e.CityName, f => f.Address.City())
			.RuleFor(e => e.Id, f => f.Random.Guid());

		return faker.Generate(count);
	}
	public IEnumerable<Client> ClientFaker(int count, IQueryable<Location> locations)
	{
		var faker = new Faker<Client>().UseSeed(1341);
		faker
			.RuleFor(e => e.Classification, f => f.PickRandom<ClientStatus>().ToString())
			.RuleFor(e => e.OfficeLocation, f => f.PickRandom<Location>(locations).CityName)
			.RuleFor(e => e.Segment, f => f.PickRandom<Segment>().ToString())
			.RuleFor(e => e.Name, f => f.Name.FullName(f.Person.Gender))
			.RuleFor(e => e.PictureUrl, f => f.Image.PicsumUrl())
			.RuleFor(e => e.Id, f => f.Random.Guid());
		return faker.Generate(count);
	}
	public IEnumerable<Contact> ContactFaker(int count)
	{
		var faker = new Faker<Contact>().UseSeed(1342);
		faker
			.RuleFor(e => e.Name, f => f.Name.FullName(f.Person.Gender))
			.RuleFor(e => e.Email, f => f.Person.Email)
			.RuleFor(e => e.PhoneNumber, f => f.Phone.PhoneNumber())
			.RuleFor(e => e.LinkedInUrl, f => f.Internet.Url())
			.RuleFor(e => e.JobTitle, f => f.Name.JobTitle())
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
			.RuleFor(e => e.Id, f => f.Random.Guid());
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
			.RuleFor(e => e.Id, f => f.Random.Guid());
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
			.RuleFor(e => e.Id, f => f.Random.Guid());
		return faker.Generate(clients.Count());
	}
}
