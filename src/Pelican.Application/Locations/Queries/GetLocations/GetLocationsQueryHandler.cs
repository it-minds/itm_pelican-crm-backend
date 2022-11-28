﻿using Pelican.Application.Abstractions.Data.Repositories;
using Pelican.Application.Abstractions.Messaging;
using Pelican.Domain.Entities;

namespace Pelican.Application.Locations.Queries.GetLocations;
public class GetLocationsQueryHandler : IQueryHandler<GetLocationsQuery, IQueryable<Location>>
{
	private readonly IGenericRepository<Location> _repository;
	public GetLocationsQueryHandler(IUnitOfWork unitOfWork)
	{
		_repository = unitOfWork.LocationRepository;
	}
	//Uses the repository for Location to find all Locations in the database
	public async Task<IQueryable<Location>> Handle(GetLocationsQuery request, CancellationToken cancellation)
	{
		return await Task.Run(() => _repository.FindAll());
	}
}