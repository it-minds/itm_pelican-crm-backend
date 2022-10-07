using Pelican.Application.Common.Interfaces;

namespace Pelican.Infrastructure.Persistence;
public static class DevelopmentSeeder
{
	public static void SeedEntireDb(IPelicanContext pelicanContext, IPelicanFaker pelicanFaker)
	{
		DevelopmentSeeder.SeedSuppliers(pelicanContext, pelicanFaker);
	}
	private static void SeedSuppliers(IPelicanContext pelicanContext, IPelicanFaker pelicanFaker)
	{
		pelicanContext.Suppliers.AddRange(pelicanFaker.SupplierFaker(5));
		pelicanContext.SaveChanges();
	}

}
