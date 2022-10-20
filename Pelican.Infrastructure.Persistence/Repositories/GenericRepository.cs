using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Pelican.Application.Common.Interfaces;
using Pelican.Application.Common.Interfaces.Repositories;
using Pelican.Domain.Primitives;

namespace Pelican.Infrastructure.Persistence.Repositories;
public class GenericRepository<T> : IGenericRepository<T> where T : Entity
{
	//This Repository contains base functions that will be inherited by all specific repositories
	protected PelicanContext PelicanContext { get; set; }
	public GenericRepository(IPelicanContext pelicanContext)
	{
		try
		{
			PelicanContext = (PelicanContext)pelicanContext;
		}
		catch (InvalidCastException e)
		{
			throw e;
		}
	}

	public IQueryable<T> FindAll() => PelicanContext.Set<T>().AsNoTracking();

	public async Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
	{
		if (id == Guid.Empty)
		{
			throw new ArgumentNullException($"{nameof(GetByIdAsync)} id must not be empty");
		}
		return await PelicanContext.Set<T>().FirstOrDefaultAsync(entity => entity.Id == id, cancellationToken);
	}

	public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression) => PelicanContext.Set<T>().Where(expression).AsNoTracking();

	public async Task<T> CreateAsync(T entity, CancellationToken cancellationToken)
	{
		if (entity is null)
		{
			throw new ArgumentNullException($"{nameof(CreateAsync)} entity must not be null");
		}
		await PelicanContext.Set<T>().AddAsync(entity, cancellationToken);
		return entity;
	}

	public async Task<IEnumerable<T>> CreateRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken)
	{
		if (entities is null)
		{
			throw new ArgumentNullException($"{nameof(CreateRangeAsync)} entity must not be null");
		}
		await PelicanContext.Set<T>().AddRangeAsync(entities, cancellationToken);
		return entities;
	}

	public void Update(T entity) => PelicanContext.Set<T>().Update(entity);

	public void Delete(T entity) => PelicanContext.Set<T>().Remove(entity);
}
