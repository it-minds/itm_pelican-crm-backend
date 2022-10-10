using Pelican.Application.Common.Interfaces;

namespace Pelican.Infrastructure.Persistence;
public static class DevelopmentSeeder
{
	//This method is only partially created it should call specific methods to seed each table in the database.
	public static void SeedEntireDb(IPelicanContext pelicanContext, IPelicanBogusFaker pelicanFaker)
	{
		DevelopmentSeeder.SeedSuppliers(pelicanContext, pelicanFaker);
	}
	//This Method calls on the PelicanBogusFaker to give it a list of 5 Supplier entities and saves these to the database.
	private static void SeedSuppliers(IPelicanContext pelicanContext, IPelicanBogusFaker pelicanFaker)
	{
		pelicanContext.Suppliers.AddRange(pelicanFaker.SupplierFaker(5));
		pelicanContext.SaveChanges();
	}

}
