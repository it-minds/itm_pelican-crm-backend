using Pelican.Application.Common.Interfaces;
using Pelican.Application.Common.Interfaces.Repositories;

namespace Pelican.Infrastructure.Persistence;
public static class DevelopmentSeeder
{
	//This method is only partially created it should call specific methods to seed each table in the database.
	public static async Task SeedEntireDb(IUnitOfWork unitOfWork, IPelicanBogusFaker pelicanFaker)
	{
		await SeedSuppliers(unitOfWork, pelicanFaker);
	}
	//This Method calls on the PelicanBogusFaker to give it a list of 5 Supplier entities and saves these to the database.
	private static async Task SeedSuppliers(IUnitOfWork unitOfWork, IPelicanBogusFaker pelicanFaker)
	{
		await unitOfWork
			.SupplierRepository
			.CreateRangeAsync(pelicanFaker.SupplierFaker(5), CancellationToken.None);
		unitOfWork.Save();
	}

}
