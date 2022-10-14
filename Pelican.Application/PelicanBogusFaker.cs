﻿using Bogus;
using Pelican.Application.Common.Interfaces;
using Pelican.Domain.Entities;
using Pelican.Domain.Enums;

namespace Pelican.Application;
public class PelicanBogusFaker : IPelicanBogusFaker
{
	public PelicanBogusFaker() { }

	public IEnumerable<AccountManager> AccountManagerFaker(int supplierCount, IQueryable<Supplier> suppliers)
	{
		var faker = new Faker<AccountManager>().UseSeed(1337);
		var random = new Random();
		int index = random.Next(suppliers.Count());
		int numberOfLocations = random.Next(5);
		faker
			.RuleFor(e => e.Name, f => f.Name.FullName(f.Person.Gender))
			.RuleFor(e => e.PictureUrl, f => f.Image.PicsumUrl())
			.RuleFor(e => e.PhoneNumber, f => f.Phone.PhoneNumber())
			.RuleFor(e => e.Email, faker => faker.Person.Email)
			.RuleFor(e => e.LinkedInUrl, f => f.Internet.Url())
			.RuleFor(e => e.Supplier, suppliers.ToList()[index])
			.RuleFor(e => e.SupplierId, suppliers.ToList()[index].Id);
		return faker.Generate(supplierCount);
	}
	public IEnumerable<Supplier> SupplierFaker(int count)
	{
		var faker = new Faker<Supplier>().UseSeed(1337);
		faker
			.RuleFor(e => e.Name, f => f.Name.FullName(f.Person.Gender))
			.RuleFor(e => e.CreatedAt, f => f.Date.PastOffset(10).ToUnixTimeMilliseconds())
			.RuleFor(e => e.Email, f => f.Person.Email)
			.RuleFor(e => e.LastUpdatedAt, f => f.Date.PastOffset(1).ToUnixTimeMilliseconds())
			.RuleFor(e => e.WebsiteUrl, f => f.Internet.Url())
			.RuleFor(e => e.LinkedInUrl, f => f.Internet.Url())
			.RuleFor(e => e.PhoneNumber, f => f.Phone.PhoneNumber())
			.RuleFor(e => e.PictureUrl, f => f.Image.PicsumUrl())
			.RuleFor(e => e.Id, f => f.Random.Guid());
		return faker.Generate(count);
	}
	public IEnumerable<Deal> DealFaker(int count, IQueryable<Client> clients)
	{
		var faker = new Faker<Deal>().UseSeed(1337);
		var random = new Random();
		int index = random.Next(clients.Count());
		faker
			.RuleFor(e => e.Revenue, f => f.Random.Decimal() * 10000)
			.RuleFor(e => e.CreatedAt, f => f.Date.PastOffset(10).ToUnixTimeMilliseconds())
			.RuleFor(e => e.DealStatus, f => f.PickRandom<DealStatus>().ToString())
			.RuleFor(e => e.LastUpdatedAt, f => f.Date.PastOffset(1).ToUnixTimeMilliseconds())
			.RuleFor(e => e.EndDate, f => Convert.ToDateTime(f.Date.PastDateOnly()))
			.RuleFor(e => e.Id, f => f.Random.Guid())
			.RuleFor(e => e.Client, clients.ToList()[index])
			.RuleFor(e => e.ClientId, clients.ToList()[index].Id);
		return faker.Generate(count);
	}
}
