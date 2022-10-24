using Microsoft.EntityFrameworkCore;
using Pelican.Application.Common.Interfaces.DataLoaders;
using Pelican.Application.Common.Interfaces.Repositories;
using Pelican.Domain.Primitives;

namespace Pelican.Infrastructure.Persistence.DataLoader;

public class GenericDataLoader<T> : BatchDataLoader<Guid, T>, IGenericDataLoader<T> where T : Entity
{
	private IUnitOfWork _unitOfWork;
	public GenericDataLoader(IBatchScheduler batchScheduler, IUnitOfWork unitOfWork) : base(batchScheduler)
	{
		_unitOfWork = unitOfWork;
	}
	//This loads a batch from the database where the id is equal to the id requested
	protected override async Task<IReadOnlyDictionary<Guid, T>> LoadBatchAsync(IReadOnlyList<Guid> keys, CancellationToken cancellationToken)
	{
		return await _unitOfWork.GetRepository<T>()
			.FindAllWithIncludes()
			.Where(s => keys.Contains(s.Id))
			.ToDictionaryAsync(t => t.Id, cancellationToken);
	}
}
