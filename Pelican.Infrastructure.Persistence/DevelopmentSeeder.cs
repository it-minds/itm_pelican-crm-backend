using Bogus;
using Pelican.Domain.Entities;

namespace Pelican.Infrastructure.Persistence;
public static class DevelopmentSeeder
{
	public static void SeedEntireDb(PelicanContext pelicanContext)
	{
		DevelopmentSeeder.SeedSuppliers(pelicanContext);
	}
	private static void SeedSuppliers(PelicanContext pelicanContext)
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
			.RuleFor(e => e.PictureUrl, f => f.Image.PicsumUrl());
		pelicanContext.Suppliers.AddRange(faker.Generate(5));
		pelicanContext.SaveChanges();
	}

}
