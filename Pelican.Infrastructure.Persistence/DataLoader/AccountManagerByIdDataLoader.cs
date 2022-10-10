using GreenDonut;
using Microsoft.EntityFrameworkCore;
using Pelican.Application.Common.Interfaces.DataLoaders;
using Pelican.Domain.Entities;
using Pelican.Infrastructure.Persistence;

public class AccountManagerByIdDataLoader : BatchDataLoader<Guid, AccountManager>, IAccountManagerByIdDataLoader
{
	private readonly IDbContextFactory<PelicanContext> _dbContextFactory;
	public AccountManagerByIdDataLoader(IBatchScheduler batchScheduler, IDbContextFactory<PelicanContext> dbContextFactory) : base(batchScheduler)
	{
		_dbContextFactory = dbContextFactory ?? throw new ArgumentNullException(nameof(dbContextFactory));
	}
	//This loads a batch from the database where the id is equal to the id requested
	protected override async Task<IReadOnlyDictionary<Guid, AccountManager>> LoadBatchAsync(IReadOnlyList<Guid> keys, CancellationToken cancellationToken)
	{
		await using var pelicanContext = _dbContextFactory.CreateDbContext();
		return await pelicanContext.Set<AccountManager>().Where(s => keys.Contains(s.Id)).ToDictionaryAsync(t => t.Id, cancellationToken);
	}
}
