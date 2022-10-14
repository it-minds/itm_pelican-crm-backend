using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Pelican.Domain.Entities;
using Pelican.Infrastructure.Google.Authentication.Interfaces;
using Pelican.Infrastructure.Google.Authentication.Models;
using Pelican.Infrastructure.Persistence;


namespace Pelican.Infrastructure.Google.Authentication.Services;

public class EmployeeClaimsService : IEmployeeClaimsService
{
	private readonly IDbContextFactory<PelicanContext> _dbContextFactory;
	public EmployeeClaimsService(IDbContextFactory<PelicanContext> dbContextFactory)
	{
		_dbContextFactory = dbContextFactory;
	}
	public async Task<EmployeeClaimData> GetEmployeeClaimDataAsync(string email, CancellationToken cancellationToken = default)
	{
		await using var pelicanContext = _dbContextFactory.CreateDbContext();
		var employee = await pelicanContext.AccountManagers
			.AsNoTracking()
			.Where(e => e.Email == email)
			.ProjectTo<EmployeeClaimData>(new MapperConfiguration(config =>
			{
				config.CreateMap<AccountManager, EmployeeClaimData>()
				.ForMember(dest => dest.EmployeeId, opts => opts.MapFrom(src => src.Id))
				.ForMember(dest => dest.EmployeeName, opts => opts.MapFrom(src => src.Name))
				.ForMember(dest => dest.Company, opts => opts.MapFrom(src => src.Supplier.Name));
			}))
			.FirstOrDefaultAsync(cancellationToken);

		return employee;
	}
}
