using Microsoft.EntityFrameworkCore;
using Pelican.Domain.Entities;
using Pelican.Infrastructure.Persistence;

public class AccountManagerByIdDataLoader : BatchDataLoader<Guid, AccountManager>
{
	private readonly IDbContextFactory<PelicanContext> _dbContextFactory;
	public AccountManagerByIdDataLoader(IBatchScheduler batchScheduler, IDbContextFactory<PelicanContext> dbContextFactory) : base(batchScheduler)
	{
		_dbContextFactory = dbContextFactory ?? throw new ArgumentNullException(nameof(dbContextFactory));
	}
	protected override async Task<IReadOnlyDictionary<Guid, AccountManager>> LoadBatchAsync(IReadOnlyList<Guid> keys, CancellationToken cancellationToken)
	{
		await using PelicanContext pelicanContext = _dbContextFactory.CreateDbContext();
		return await pelicanContext.AccountManagers.Where(s => keys.Contains(s.Id)).ToDictionaryAsync(t => t.Id, cancellationToken);
	}
}
